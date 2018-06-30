﻿using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CentennialTalk.Main.Controllers
{
    [Route("api/message")]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly IUnitOfWorkService uowService;

        public MessageController(IMessageService messageService,
            IUnitOfWorkService uowService)
        {
            this.messageService = messageService;
            this.uowService = uowService;
        }

        [HttpPost("messages")]
        public IActionResult GetChatMessages(string chatCode)
        {
            List<Message> messages = messageService.GetChatMessages(chatCode);

            return GetJson(new ResponseDTO(ResponseCode.OK, messages));
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody]MessageDTO messageData)
        {
            messageService.SaveMessage(messageData);

            bool saved = uowService.SaveChanges();

            return GetJson(new ResponseDTO(saved ? ResponseCode.OK : ResponseCode.ERROR,
                saved ? "Message saved" : "Error while saving message"));
        }
    }
}