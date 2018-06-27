using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CentennialTalk.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly ILogger<MessageService> logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            this.messageRepository = messageRepository;
            this.logger = logger;
        }

        public List<Message> GetChatMessages(string chatCode)
        {
            try
            {
                return messageRepository.GetChatMessages(chatCode);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }

        public bool SaveMessage(MessageDTO messageData)
        {
            try
            {
                Message message = new Message(messageData);

                //if (messageData.replyId != Guid.Empty)
                //    message.SetReplyId(messageData.replyId);

                return messageRepository.SaveMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return false;
            }
        }
    }
}