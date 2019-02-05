using CentennialTalk.Models;
using CentennialTalk.Models.QuestionModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CentennialTalk.Service
{
    public class FileService : IFileService
    {
        private readonly IChatRepository chatRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IQuestionRepository quesRepository;
        private readonly IQuestionService quesService;
        private readonly ILogger<ChatService> logger;

        public FileService(IChatRepository chatRepository, IMessageRepository messageRepository,
            IQuestionRepository quesRepository, IQuestionService quesService, ILogger<ChatService> logger)
        {
            this.chatRepository = chatRepository;
            this.messageRepository = messageRepository;
            this.quesRepository = quesRepository;
            this.quesService = quesService;
            this.logger = logger;
        }

        public string CreateWordDocument(string chatCode)
        {
            try
            {
                Discussion chat = chatRepository.GetChatByCode(chatCode, true);

                if (chat == null)
                    return null;

                DocumentCore docx = new DocumentCore();

                Section messageSection = WriteMessagesToDoc(chat, docx);
                docx.Sections.Add(messageSection);

                Section pollSection = WritePollAnswersToDoc(chat, docx);
                docx.Sections.Add(pollSection);

                List<ClusteredResponses> res = GetClusteredQuestions(chat);

                if (res != null && res.Count > 0)
                {
                    Section openEndedSection = WriteOpenQuestionsWithClusters(chat, docx, res);
                    docx.Sections.Add(openEndedSection);
                }
                else
                {
                    Section openEndedSection = WriteOpenQuestionsWithoutClustering(chat, docx);
                    docx.Sections.Add(openEndedSection);
                }

                string fileName = chat.Title + "_" + chat.DiscussionCode + "_" + DateTime.Now.ToString()
                    .Replace("/", string.Empty)
                    .Replace(" ", string.Empty)
                    .Replace(":", string.Empty) + ".docx";

                docx.Save(fileName);

                return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Section WriteOpenQuestionsWithoutClustering(Discussion chat, DocumentCore docx)
        {
            Section openEndedSection = new Section(docx);

            openEndedSection.PageSetup.PaperType = PaperType.A4;

            List<SubjectiveQuestion> subs = quesRepository.GetChatSubjectiveQuestions(chat.DiscussionCode);

            List<UserAnswer> answers = quesRepository.GetAnswersByChat(chat.DiscussionCode);

            foreach (SubjectiveQuestion sub in subs)
            {
                Paragraph pq = new Paragraph(docx);

                pq.Content.End.Insert(string.Format("Subjective Question : {0}", sub.Content),
                    new CharacterFormat() { Size = 12, FontColor = Color.Blue, Bold = true });

                pq.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                pq.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                foreach (GroupMember mem in chat.Members)
                {
                    UserAnswer quesans = answers.FirstOrDefault(x => x.QuestionId == sub.QuestionId && x.MemberId == mem.GroupMemberId);

                    if (quesans == null)
                        continue;

                    bool isMod = mem.IsModerator;

                    Paragraph p = new Paragraph(docx);

                    p.Content.End.Insert(string.Format("{0} Answers : ", mem.Username), new CharacterFormat() { Size = 12, FontColor = Color.Green });

                    p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    p.Content.End.Insert(quesans.Content, new CharacterFormat() { Size = 12, FontColor = Color.Green });

                    p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    p.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                    openEndedSection.Blocks.Add(p);
                }

                openEndedSection.Blocks.Add(pq);
            }

            //docx.Sections.Add(openEndedSection);

            return openEndedSection;
        }

        private Section WriteOpenQuestionsWithClusters(Discussion chat, DocumentCore docx, List<ClusteredResponses> res)
        {
            Section openEndedSection = new Section(docx);

            openEndedSection.PageSetup.PaperType = PaperType.A4;

            List<SubjectiveQuestion> subs = quesRepository.GetChatSubjectiveQuestions(chat.DiscussionCode);

            List<UserAnswer> answers = quesRepository.GetAnswersByChat(chat.DiscussionCode);

            foreach (Guid qid in res.Select(x => x.QuestionId).Distinct().ToList())
            {
                SubjectiveQuestion sub = subs.First(x => x.QuestionId == qid);

                if (sub == null)
                    continue;

                List<ClusteredResponses> filterByQuestion = res.Where(x => x.QuestionId == qid).ToList();

                if (filterByQuestion == null || filterByQuestion.Count <= 0)
                    continue;

                Paragraph pq = new Paragraph(docx);

                pq.Content.End.Insert(string.Format("Subjective Question : {0}", sub.Content),
                    new CharacterFormat() { Size = 12, FontColor = Color.Blue, Bold = true });

                pq.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                pq.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                foreach (uint clid in filterByQuestion.Select(x => x.PredictedClusterId).Distinct().ToList())
                {
                    foreach (ClusteredResponses responses in filterByQuestion)
                    {
                        List<ClusteredResponses> filterByCluster = filterByQuestion.Where(x => x.PredictedClusterId == clid).ToList();

                        if (filterByCluster == null || filterByCluster.Count <= 0)
                            continue;

                        Paragraph p = new Paragraph(docx);

                        foreach (ClusteredResponses resByCluster in filterByCluster)
                        {
                            UserAnswer quesans = answers.FirstOrDefault(x => x.Id == resByCluster.ResponseId);

                            if (quesans == null)
                                continue;

                            GroupMember mem = chat.Members.First(x => x.GroupMemberId == resByCluster.MemberId);

                            bool isMod = mem.IsModerator;

                            p.Content.End.Insert(string.Format("{0} Answers : ", mem.Username), new CharacterFormat() { Size = 12, FontColor = Color.Green });

                            p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                            p.Content.End.Insert(quesans.Content, new CharacterFormat() { Size = 12, FontColor = Color.Green });

                            p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                            p.ParagraphFormat.Alignment = HorizontalAlignment.Left;
                        }

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                        openEndedSection.Blocks.Add(p);
                    }
                }

                openEndedSection.Blocks.Add(pq);
            }

            return openEndedSection;
        }

        private Section WritePollAnswersToDoc(Discussion chat, DocumentCore docx)
        {
            Section pollSection = new Section(docx);
            pollSection.PageSetup.PaperType = PaperType.A4;

            List<PollingQuestion> polls = quesRepository.GetChatPollingQuestions(chat.DiscussionCode);

            List<UserAnswer> answers = quesRepository.GetAnswersByChat(chat.DiscussionCode);

            foreach (PollingQuestion poll in polls)
            {
                Paragraph pq = new Paragraph(docx);

                pq.Content.End.Insert(string.Format("Polling Question(Select {0}) : {1}",
                    poll.SelectMultiple ? "Multiple" : "One",
                    poll.Content), new CharacterFormat() { Size = 12, FontColor = Color.Blue, Bold = true });

                pq.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                poll.Options.ToList().ForEach(o =>
                {
                    pq.Content.End.Insert(o.Text, new CharacterFormat() { Size = 12, FontColor = Color.Blue, Bold = true });

                    pq.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));
                });

                pq.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                foreach (GroupMember mem in chat.Members)
                {
                    List<UserAnswer> quesans = answers.FindAll(x => x.QuestionId == poll.QuestionId && x.MemberId == mem.GroupMemberId);

                    if (quesans == null || quesans.Count <= 0)
                        continue;

                    bool isMod = mem.IsModerator;

                    Paragraph p = new Paragraph(docx);

                    p.Content.End.Insert(string.Format("{0} Answers : ", mem.Username), new CharacterFormat() { Size = 12, FontColor = Color.Green });

                    p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    quesans.ToList().ForEach(o =>
                    {
                        p.Content.End.Insert(o.Content, new CharacterFormat() { Size = 12, FontColor = Color.Green });

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));
                    });

                    p.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                    pollSection.Blocks.Add(p);
                }

                pollSection.Blocks.Add(pq);
            }

            return pollSection;
        }

        private Section WriteMessagesToDoc(Discussion chat, DocumentCore docx)
        {
            List<Message> messages = messageRepository.GetChatMessages(chat.DiscussionCode);

            Section messageSection = new Section(docx);

            messageSection.PageSetup.PaperType = PaperType.A4;

            foreach (Message m in messages)
            {
                GroupMember mem = chat.Members.FirstOrDefault(x => x.Username == m.Sender);

                bool isMod = mem.IsModerator;

                Paragraph p = new Paragraph(docx,
                    string.Format("{0} ({2}) : " +
                    "{1} ", m.Sender, m.Content, m.SentDate.ToString()));

                p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                p.ParagraphFormat.Alignment = isMod ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                messageSection.Blocks.Add(p);
            }

            return messageSection;
        }

        public List<ClusteredResponses> GetClusteredQuestions(Discussion chat)
        {
            try
            {
                List<ClusteredResponses> clusters = new List<ClusteredResponses>();

                List<SubjectiveQuestion> subs = quesRepository.GetChatSubjectiveQuestions(chat.DiscussionCode);

                List<UserAnswer> answers = quesRepository.GetAnswersByChat(chat.DiscussionCode);

                foreach (SubjectiveQuestion sub in subs)
                {
                    foreach (GroupMember mem in chat.Members)
                    {
                        UserAnswer quesans = answers.FirstOrDefault(x => x.QuestionId == sub.QuestionId && x.MemberId == mem.GroupMemberId);

                        if (quesans == null)
                            continue;

                        ResponseTrainingModel arg = new ResponseTrainingModel();
                        arg.Question = sub.Content;
                        arg.Answer = quesans.Content;

                        ClusterPrediction pred = quesService.TrainModel(arg);

                        ClusteredResponses res = new ClusteredResponses();

                        res.PredictedClusterId = pred.PredictedClusterId;
                        res.QuestionId = sub.QuestionId;
                        res.ResponseContent = quesans.Content;
                        res.ResponseId = quesans.Id;
                        res.MemberId = quesans.MemberId;

                        clusters.Add(res);
                    }
                }

                return clusters;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void CommentedCode()
        {
            /*
             foreach (SubjectiveQuestion sub in subs)
                {
                    Paragraph pq = new Paragraph(docx);

                    pq.Content.End.Insert(string.Format("Subjective Question : {0}", sub.Content),
                        new CharacterFormat() { Size = 12, FontColor = Color.Blue, Bold = true });

                    pq.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    pq.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                    foreach (GroupMember mem in chat.Members)
                    {
                        UserAnswer quesans = answers.FirstOrDefault(x => x.QuestionId == sub.QuestionId && x.MemberId == mem.GroupMemberId);

                        if (quesans == null)
                            continue;

                        bool isMod = mem.IsModerator;

                        Paragraph p = new Paragraph(docx);

                        ResponseTrainingModel arg = new ResponseTrainingModel();
                        arg.Question = sub.Content;
                        arg.Answer = quesans.Content;

                        ClusterPrediction pred = quesService.TrainModel(arg);

                        p.Content.End.Insert(string.Format("{0} Answers : ", mem.Username), new CharacterFormat() { Size = 12, FontColor = Color.Green });

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                        p.Content.End.Insert(quesans.Content, new CharacterFormat() { Size = 12, FontColor = Color.Green });

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                        p.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                        sec2.Blocks.Add(p);
                    }

                    sec3.Blocks.Add(pq);
                }
             */
        }
    }
}