using System.Net.Http.Headers;
using devagramC.Dtos;

namespace devagramC.Services
{
    public class CosmicService
    {

        public string EnviarImagem(ImagemDto imagemdto)
        {
            Stream imagem = imagemdto.Imagem.OpenReadStream();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "CnClQZRjWULWP1pOhrVU7rMLxLDoIzML7lv09QZBf9AZLuUKL6");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            var conteudo = new MultipartFormDataContent
            {
                {new StreamContent(imagem), "media", imagemdto.Nome }
            };


            request.Content = conteudo;

            var retornoreq = client.PostAsync("https://workers.cosmicjs.com/v3/buckets/devagram-csharp-production/media", request.Content).Result;

            var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRespostaDto>();

            return urlretorno.Result.media.url;
        }
    }
}
