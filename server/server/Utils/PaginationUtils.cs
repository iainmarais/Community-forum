namespace RestApiServer.Utils
{
    public static class PaginationUtils
    {
        public static int GetTotalPageCount(int totalRows, int RowsPerPage)
        {
            return (int)Math.Ceiling((double)totalRows / RowsPerPage);
        }
    }
}