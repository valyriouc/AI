// namespace VectorDbs.ChromaDb;
//
// public class ChromaRequestMessage : HttpRequestMessage
// {
//     private ChromaRequestMessage(
//         string baseUrl,
//         string path,
//         HttpMethod method)
//     {
//         this.RequestUri = new Uri(new Uri(baseUrl), path);
//         this.Method = method;
//     }
//
//     public static ChromaRequestMessage CreateDatabaseAsync(string baseUrl)
//     {
//         return new ChromaRequestMessage(
//             baseUrl,
//             "api/v1/databases",
//             )
//     } 
//     
// }