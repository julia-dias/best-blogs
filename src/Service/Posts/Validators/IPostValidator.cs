using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Posts.Validators
{
    public interface IPostValidator
    {
        Task ValidatePostExistsAsync(Guid postId);
    }
}
