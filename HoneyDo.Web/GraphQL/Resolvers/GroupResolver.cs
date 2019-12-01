using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HoneyDo.Web.Extensions;
using HoneyDo.Web.Models;
using HotChocolate.Resolvers;

namespace HoneyDo.Web.GraphQL.Resolvers
{
    public class GroupResolver
    {
        private readonly GroupService _groupService;

        public GroupResolver(GroupService groupService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        /// <summary> Gets all the groups that the user has access to.</summary>
        public async Task<List<Group>> Groups() => await _groupService.Get();

        /// <summary> Gets a specific group.  </summary>
        public async Task<Group> Group(Guid groupId, IResolverContext ctx) =>
            (await _groupService.Get(groupId)).ForGraphQL(ctx);

        /// <summary> Create group. </summary>
        public async Task<Group> CreateGroup(GroupCreateForm input, IResolverContext ctx) => 
            (await _groupService.Create(input)).ForGraphQL(ctx);

        /// <summary> Update group. </summary>
        public async Task<Group> UpdateGroup(Guid groupId, GroupCreateForm input, IResolverContext ctx) =>
            (await _groupService.Update(groupId, input)).ForGraphQL(ctx);

        /// <summary> Delete group. </summary>
        public async Task<bool> DeleteGroup(Guid groupId, IResolverContext ctx) => 
            (await _groupService.Delete(groupId)).ForGraphQL(ctx);

        /// <summary> Add accounts to group. </summary>
        public async Task<GroupAccount[]> AddAccounts(Guid groupId, Guid[] accountIds, IResolverContext ctx) =>
            (await _groupService.AddAccounts(groupId, accountIds)).ForGraphQL(ctx);

        /// <summary> Add accounts to group. </summary>
        public async Task<bool> RemoveAccounts(Guid groupId, Guid[] accountIds, IResolverContext ctx) =>
            (await _groupService.RemoveAccounts(groupId, accountIds)).ForGraphQL(ctx);
    }
}
