namespace FileStorage.Shared.Common.Exceptions
{
    public class FileStorageErrorMessage
    {
        public FileStorageErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"کد پیام: {Code}\t متن پیام: {Message}";
        }

        public FileStorageErrorMessage AddSqlErrorCode(int sqlErrorCode)
        {
            return new FileStorageErrorMessage(Code + $" ({sqlErrorCode}) ", Message);
        }
    }
}