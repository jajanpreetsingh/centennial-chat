using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using SautinSoft.Document;
using System.Collections.Generic;

namespace CentennialTalk.ServiceContract
{
    public interface IChatService
    {
        Discussion CreateNewChat(NewChatDTO newChat);

        Discussion GetChatByCode(string code);

        ResponseDTO JoinChat(JoinChatDTO joinChat);

        List<Discussion> GetOpenChatGroups();

        DocumentCore CreateWordDocument(string chatCode);
    }
}