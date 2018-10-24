using PhotoShare.Client.Core.Contracts;
using PhotoShare.Services.Contracts;

namespace PhotoShare.Client.Core.Commands
{
    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;

        public AddTagCommand(ITagService tagService)
        {
            this.tagService = tagService; 
        }

        public string Execute(string[] args)
        {
            string tag = args[0];

            tagService.AddTag(tag);
        }
    }
}
