using School.ViewModels.Common;

namespace School.ViewModels.Catalog.Course.Request
{
    public class PagingCourseViewModelRequest : PagingRequestBase
    {
        public string Title { get; set; }
    }
}
