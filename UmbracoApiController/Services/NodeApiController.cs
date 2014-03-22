﻿using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using umbraco;
using umbraco.NodeFactory;
using Umbraco.Web.WebApi;
using UmbracoTest.Services.Models;

namespace UmbracoTest.Services
{
    [AllowCrossSiteJson]
    public class NodeApiController : UmbracoApiController
    {       
        /// <summary>
        /// Gets the umbraco node by id
        /// </summary>
        public HttpResponseMessage GetNodeData(int id)
        {
            var node = new Node(id);

            if (node.Id == 0)
                return NoteNodeFound();
            

            var viewNode = ViewNode.Create(node);

            return JsonResponse(viewNode);
        }

        /// <summary>
        /// Gets the umbraco node by url, example : /about        
        /// </summary>
        public HttpResponseMessage GetNodeByUrl(string url)
        {
            var node = uQuery.GetNodeByUrl(url);

            //we don't want the root node, use GetNodeData for that
            if (node.Id == 0 || node.Id == -1)
                return NoteNodeFound();

            var viewNode = ViewNode.Create(node);

            return JsonResponse(viewNode);
        }

        /// <summary>
        /// Create response object
        /// </summary>
        private HttpResponseMessage JsonResponse(object obj)
        {
            return new HttpResponseMessage
            {
                Content = JsonContent(obj),                
            };
        }

        /// <summary>
        /// Serialize object to json
        /// </summary>
        private StringContent JsonContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Node not found response
        /// </summary>
        private HttpResponseMessage NoteNodeFound()
        {
            return JsonResponse(new ViewNode()
            {
                StatusMessage = new StatusMessage { Success = false, Message = "Node not found" }
            });
        }
    }
}