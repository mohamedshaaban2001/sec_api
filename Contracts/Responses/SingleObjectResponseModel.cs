namespace Contracts.Responses
{
    public class SingleObjectResponseModel<T> : ParentResponseModel
    {
        /// <summary>
        /// singleObject returened from the database to the end point
        /// </summary>
        public T SingleObject { get; set; }
    }
}
