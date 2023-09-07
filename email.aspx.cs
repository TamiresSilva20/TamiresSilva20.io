using System;
using System.IO;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;

namespace testeemail
{
    public partial class email : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Obsolete]
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            // Diretório para armazenar as credenciais do Gmail
            string credenciaisDiretorio = Server.MapPath("~");

            // Caminho para o arquivo JSON de credenciais
            string jsonCredenciaisPath = Path.Combine(credenciaisDiretorio, "C:/Users/tamir/Downloads/apiEmail/client_secret_181871436285-dpemok0as2oocavsdq3ock2ieg1b13v7.apps.googleusercontent.com.json");

            // Escopo da API do Gmail
            string[] scopes = { GmailService.Scope.GmailSend };

            try
            {
                UserCredential credential;

                using (var stream = new FileStream(jsonCredenciaisPath, FileMode.Open, FileAccess.Read))
                {
                    // Carregue as credenciais do arquivo JSON
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credenciaisDiretorio, true)).Result;
                }

                // Crie a instância do serviço Gmail
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "enviaemailweb"
                });

                // Endereço de e-mail do destinatário
                string destinatarioEmail = txtEnviaEmail.Text;

                // Crie a mensagem de e-mail
                var mensagem = new MimeMessage();
                mensagem.From.Add(new MailboxAddress("Milly", "army71006@gmail.com"));
                mensagem.To.Add(new MailboxAddress("", destinatarioEmail));
                mensagem.Subject = "Teste";
                var corpo = new TextPart("plain")
                {
                    Text = "ola isso e um teste"
                };

                // Crie uma parte multipart para a mensagem
                var multipart = new Multipart("mixed");

                // Adicione o corpo do e-mail à parte multipart
                multipart.Add(corpo);

                // Anexe a planilha ao e-mail
                var anexo = new MimePart("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Content = new MimeContent(File.OpenRead("C:/Users/tamir/Downloads/CriaEco.xlsx"), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "CriaEco.xlsx"
                };

                // Adicione o anexo à parte multipart
                multipart.Add(anexo);

                // Atribua a parte multipart como corpo da mensagem
                mensagem.Body = multipart;

                // Converta a mensagem para o formato Gmail
                var msgStr = mensagem.ToString();

                // Crie uma nova mensagem do Gmail
                var gmailMessage = new Message
                {
                    Raw = Base64UrlEncode(msgStr)
                };

                // Envie o e-mail
                var envioRequest = service.Users.Messages.Send(gmailMessage, "me").Execute();

                // Exiba uma mensagem de sucesso
                lblMensagem.Text = "E-mail enviado com sucesso!";
            }
            catch (Exception ex)
            {
                // Lide com erros e exiba mensagens de erro, se necessário
                lblMensagem.Text = "Erro ao enviar o e-mail: " + ex.Message;
            }
        }

        // Função para codificar a mensagem para o formato Gmail
        private string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
