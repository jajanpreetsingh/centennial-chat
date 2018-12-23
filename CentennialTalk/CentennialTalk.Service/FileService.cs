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
        private readonly ILogger<ChatService> logger;

        public FileService(IChatRepository chatRepository, IMessageRepository messageRepository,
            IQuestionRepository quesRepository, ILogger<ChatService> logger)
        {
            this.chatRepository = chatRepository;
            this.messageRepository = messageRepository;
            this.quesRepository = quesRepository;
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

                List<Message> messages = messageRepository.GetChatMessages(chatCode);

                Section sec1 = new Section(docx);

                sec1.PageSetup.PaperType = PaperType.A4;

                foreach (Message m in messages)
                {
                    GroupMember mem = chat.Members.FirstOrDefault(x => x.Username == m.Sender);

                    bool isMod = mem.IsModerator;

                    Paragraph p = new Paragraph(docx,
                        string.Format("Sent by : {0}" +
                        "{1} " +
                        "Sent on : {2}", m.Sender, m.Content, m.SentDate.ToString()));

                    p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                    p.ParagraphFormat.Alignment = isMod ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                    sec1.Blocks.Add(p);
                }

                Section sec2 = new Section(docx);

                sec2.PageSetup.PaperType = PaperType.A4;

                List<PollingQuestion> polls = quesRepository.GetChatPollingQuestions(chatCode);

                List<UserAnswer> answers = quesRepository.GetAnswersByChat(chatCode);

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

                        sec2.Blocks.Add(p);
                    }

                    sec2.Blocks.Add(pq);
                }

                Section sec3 = new Section(docx);

                sec3.PageSetup.PaperType = PaperType.A4;

                List<SubjectiveQuestion> subs = quesRepository.GetChatSubjectiveQuestions(chatCode);

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

                        bool isMod = mem.IsModerator;

                        Paragraph p = new Paragraph(docx);

                        p.Content.End.Insert(string.Format("{0} Answers : ", mem.Username), new CharacterFormat() { Size = 12, FontColor = Color.Green });

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                        p.Content.End.Insert(quesans.Content, new CharacterFormat() { Size = 12, FontColor = Color.Green });

                        p.Inlines.Add(new SpecialCharacter(docx, SpecialCharacterType.LineBreak));

                        p.ParagraphFormat.Alignment = HorizontalAlignment.Left;

                        sec2.Blocks.Add(p);
                    }

                    sec3.Blocks.Add(pq);
                }

                docx.Sections.Add(sec1);
                docx.Sections.Add(sec2);
                docx.Sections.Add(sec3);

                string fileName = chat.Title + "_" + DateTime.Now.ToString();

                docx.Save(fileName);

                return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}