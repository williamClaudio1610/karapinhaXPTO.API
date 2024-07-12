using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net;
using KarapinhaXpto.Model;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace KarapinhaXpto.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "williamazevedo1016@gmail.com";
        private readonly string _smtpPass = "wsqn djvt tbbl jlwo";
        private readonly string _adminEmail = "williamazevedo1016@gmail.com";   

        public async Task SendNewUserNotificationAsync(string userEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("KarapinhaXPTO", _smtpUser));
            message.To.Add(new MailboxAddress("Admin", _adminEmail));
            message.Subject = "Novo Utilizador: ";
            message.Body = new TextPart("plain")
            {
                Text = $"Uma nova conta por ativar: ({userEmail})"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

            public async Task SendEmailAsync(string toEmail, string subject, string message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("KarapinhaXPTO", _smtpUser));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("plain") { Text = message };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }


        public async Task SendConfirmationEmailAsync(Marcacao marcacao)
        {
            if (marcacao.Utilizador == null || string.IsNullOrEmpty(marcacao.Utilizador.Email))
            {
                throw new ArgumentException("O utilizador associado à marcação é inválido ou o email está vazio.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("KarapinhaXPTO", _smtpUser));
            message.To.Add(new MailboxAddress(marcacao.Utilizador.NomeCompleto, marcacao.Utilizador.Email));
            message.Subject = "Confirmação de Marcação - KarapinhaXPTO";

            var builder = new BodyBuilder();

            // Saudação personalizada
            builder.TextBody = $"Olá {marcacao.Utilizador.NomeCompleto},\n\n";
            builder.TextBody += "Marcação confirmada. Abaixo os detalhes da sua marcação:\n\n";

            // Detalhes gerais da marcação
            builder.TextBody += $"Data da Marcação: {marcacao.DataRegistro:dd/MM/yyyy HH:mm}\n";
            builder.TextBody += $"Total a Pagar: {marcacao.TotalPagar:C}\n";
            builder.TextBody += $"Status: {(marcacao.Status ? "Confirmado" : "Pendente")}\n\n";

            // Detalhes dos serviços marcados
            builder.TextBody += "Detalhes dos Serviços Marcados:\n";
            foreach (var servicoMarcado in marcacao.ServicosMarcados)
            {
                builder.TextBody += $"- Serviço: {servicoMarcado.Servico.Nome}\n";
                builder.TextBody += $"  Categoria: {servicoMarcado.Servico.Categoria.Nome}\n";
                builder.TextBody += $"  Data e Hora do Serviço: {servicoMarcado.Data:dd/MM/yyyy} às {servicoMarcado.Hora:hh\\:mm}\n\n";
            }

            // Mensagem de agradecimento e contatos
            builder.TextBody += "Obrigado por escolher a KarapinhaXPTO.\n";
            builder.TextBody += "Se você tiver alguma dúvida, entre em contato connosco.\n\n";
            builder.TextBody += "Atenciosamente,\n";
            builder.TextBody += "Equipe KarapinhaXPTO";

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }



    }


}




