namespace NexChip.SignMessage.Token
{
    public class JwtAuthConfigModel
    {
        /// <summary>
        /// jwt 密钥加密key
        /// </summary>
        public readonly string JWTSecretKey = "nexchpsignmessage";

        /// <summary>
        /// 密钥过期年份
        /// </summary>
        public readonly int expYear = 10; 
    }
}