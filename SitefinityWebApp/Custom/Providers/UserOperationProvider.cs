using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Web.Services.Contracts.Operations;

namespace SitefinityWebApp.Custom.Providers
{
    public class UserOperationProvider : IOperationProvider
    {
        public IEnumerable<OperationData> GetOperations(Type clrType)
        {
            if (clrType == null)
            {
                var operation = OperationData.Create(GetUsers);
                operation.OperationType = OperationType.Unbound;
                operation.IsAllowedUnauthorized = true;
                operation.IsRead = true;
                return new OperationData[] { operation };
            }
            return Enumerable.Empty<OperationData>();
        }

        private IEnumerable<ResponseDTO> GetUsers(OperationContext context)
        {
            var userManager = UserManager.GetManager();
            var profileManager = UserProfileManager.GetManager();
            IQueryable<UserDTO> users = userManager.GetUsers().
                Select(u=>profileManager.GetUserProfile<SitefinityProfile>(u)).Select(p=>new UserDTO(p));

            return new List<ResponseDTO>()
            {
                new ResponseDTO()
            };
        }
    }
    [DataContract]
    public class ResponseDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Data { get; set; }

    }
    [DataContract]
    public class UserDTO
    {
        private SitefinityProfile P;

        public UserDTO(SitefinityProfile p)
        {
            P = p;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Data { get; set; }

    }

}