using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.IO;

namespace FlexBot
{
    public class FCommand
    {
        public string name;
        public List<string> Lines;

        public void Build(ref DiscordClient _client)
        {
            _client.GetService<CommandService>().CreateCommand(name)
                    .Description("Fact Command")
                    .Parameter("empty", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        Random r = new Random();
                        await e.Channel.SendMessage(Lines[r.Next(0,Lines.Count)]);

                    });
        }
    }

    class FactCommands
    {
        List<FCommand> Com = new List<FCommand>();
        public void Load(ref DiscordClient _client, string path)
        {
            FCommand newCom = new FCommand();
            newCom.Lines = new List<string>();
            //build commands

            DirectoryInfo Di = new DirectoryInfo(path);
            FileInfo[] fi = Di.GetFiles();
            for (int f = 0; f < fi.Length; f++)
            {
                if (fi[f].Extension == ".dbc")
                {
                    StreamReader sr = new StreamReader(fi[f].FullName);
                    while (!sr.EndOfStream)
                    {
                        string L = sr.ReadLine();
                        string[] s = L.Split("=".ToCharArray());
                        switch (s[0].Trim())
                        {
                            case "Name":
                                newCom = new FCommand();
                                newCom.Lines = new List<string>();
                                newCom.name = s[1].Replace(" ", "");
                                break;
                            case "Line":
                                newCom.Lines.Add(s[1]);
                                break;
                            case "End":
                                Com.Add(newCom);
                                break;
                        }
                    }
                    sr.Close();
                }
            }

            for (int i = 0; i < Com.Count; i++)
            {
                Com[i].Build(ref _client);
            }
        }
    }
}
