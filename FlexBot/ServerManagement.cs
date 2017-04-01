using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using IniParser;
using IniParser.Model;
using System.IO;

namespace FlexBot
{
	class SM_User
	{
		public ulong id = 0;
		public short Warning = 0;
		public short reputation = 0;
		public short repMonth = 0;
		public short repDay = 0;
		public List<ulong> autoRoles = new List<ulong>();

		public bool Exists(string folder)
		{
			return File.Exists(folder + "/" + id + ".bin");
		}

		public void write(string folder)
		{
			BinaryWriter bw = new BinaryWriter(File.Open(folder + "/" + id + ".bin", FileMode.Create));
			bw.Write(id);
			bw.Write(Warning);
			bw.Write(reputation);
			bw.Write(repMonth);
			bw.Write(repDay);
			bw.Write(autoRoles.Count);
			for (int i = 0; i < autoRoles.Count; i++)
				bw.Write(autoRoles[i]);	  
			bw.Close();
		}

		public void read(string folder)
		{
			BinaryReader br = new BinaryReader(File.Open(folder + "/" + id + ".bin", FileMode.Open));
			id = br.ReadUInt64();
			Warning = br.ReadInt16();
			reputation = br.ReadInt16();
			repMonth = br.ReadInt16();
			repDay = br.ReadInt16();
			int Count = br.ReadInt32();
			autoRoles = new List<ulong>();
			for (int i = 0; i < Count; i++)
				autoRoles.Add(br.ReadUInt64());
			br.Close();
		}

	}

	class ServerManagement
	{


		public string mute;
		public string unmute;
        public string remove;
        public string allow;
		public string clear;
		public string warning;
		public string warning_change;
		public string warning_max;
		public string dataFolder;

		public ServerManagement(ref IniData settings)
		{
			mute = settings["Server_Management"]["Mute"];
			unmute = settings["Server_Management"]["Unmute"];
            remove = settings["Server_Management"]["Remove"];
            allow = settings["Server_Management"]["Allow"];
            clear = settings["Server_Management"]["Clear"];
			warning = settings["Warning"]["Get"];
			warning_change = settings["Warning"]["Change"];
			warning_max = settings["Warning"]["Max"];
			dataFolder = settings["Folders"]["User_Data"];

          

        }

		public void BasicCommands(ref DiscordClient _client)
		{

			_client.GetService<CommandService>().CreateCommand("Mute")
			.Description("Mutes Member in a Channel")
			       .Parameter("Mentioned", ParameterType.Multiple)
			.Do(async e =>
			{
				List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();

				if (e.User.ServerPermissions.Administrator || e.User.ServerPermissions.KickMembers)
				{
					ChannelPermissionOverrides cpo = new ChannelPermissionOverrides(PermValue.Inherit, PermValue.Inherit,
						PermValue.Inherit, PermValue.Deny, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit
						, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit
						);

					foreach (User u in Mentioned)
					{
						await e.Channel.AddPermissionsRule(u, cpo);
						await e.Channel.SendMessage(mute.Replace("{mention}", u.Name));
					}
				}

			});



			_client.GetService<CommandService>().CreateCommand("Unmute")
		   .Description("Unmutes Member in a Channel")
			       .Parameter("Mentioned", ParameterType.Multiple)
		   .Do(async e =>
		   {
			   List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();

			   if (e.User.ServerPermissions.Administrator || e.User.ServerPermissions.KickMembers)
			   {
				   ChannelPermissionOverrides cpo = new ChannelPermissionOverrides(PermValue.Inherit, PermValue.Inherit,
						PermValue.Inherit, PermValue.Allow, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit
						, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit
						);

				   foreach (User u in Mentioned)
				   {
					   await e.Channel.AddPermissionsRule(u, cpo);
					   await e.Channel.SendMessage(unmute.Replace("{mention}", u.Name));
				   }


			   }


		   });

			_client.GetService<CommandService>().CreateCommand("Allow")
			.Description("Mutes Member in a Channel")
				   .Parameter("Mentioned", ParameterType.Multiple)
			.Do(async e =>
			{
				List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();

				if (e.User.ServerPermissions.Administrator || e.User.ServerPermissions.BanMembers)
				{
					ChannelPermissionOverrides cpo = new ChannelPermissionOverrides(PermValue.Inherit, PermValue.Inherit,
						PermValue.Allow, PermValue.Allow, PermValue.Inherit, PermValue.Inherit, PermValue.Allow, PermValue.Allow, PermValue.Allow, PermValue.Allow
						, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit

						);

					foreach (User u in Mentioned)
					{
						await e.Channel.AddPermissionsRule(u, cpo);
						await e.Channel.SendMessage(allow.Replace("{mention}", u.Name));
					}
				}

			});

			_client.GetService<CommandService>().CreateCommand("Remove")
			.Description("Mutes Member in a Channel")
				   .Parameter("Mentioned", ParameterType.Multiple)
			.Do(async e =>
			{
				List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();

				if (e.User.ServerPermissions.Administrator || e.User.ServerPermissions.BanMembers)
				{
					ChannelPermissionOverrides cpo = new ChannelPermissionOverrides(PermValue.Inherit, PermValue.Inherit,
                        PermValue.Deny, PermValue.Deny, PermValue.Inherit, PermValue.Inherit, PermValue.Deny, PermValue.Deny, PermValue.Deny, PermValue.Deny
						, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Inherit
						);

					foreach (User u in Mentioned)
					{
						await e.Channel.AddPermissionsRule(u, cpo);
						await e.Channel.SendMessage(remove.Replace("{mention}", u.Name));
					}
				}

			});

			_client.GetService<CommandService>().CreateCommand("Clear")
		   .Description("clears chat of messages by amount given")
		   .Parameter("Amount", ParameterType.Required)
		   .Do(async e =>
		   {
			   if (e.User.ServerPermissions.Administrator || e.User.ServerPermissions.ManageMessages)
			   {
				   int num = 0;
				   int Count = 0;
				   if (int.TryParse(e.GetArg("Amount"), out num))
				   {
					   num += 1;
					   do
					   {
						   if (num > 100)
						   {
							   Message[] m = await e.Channel.DownloadMessages(100, null, Relative.Before, true);
							   Count += m.Length;
							   await e.Channel.DeleteMessages(m);
							   if (m.Length < 100)
								   break;
						   }
						   else
						   {
							   Message[] m = await e.Channel.DownloadMessages(num, null, Relative.Before, true);
							   Count += m.Length;
							   await e.Channel.DeleteMessages(m);
						   }


						   num -= 100;
					   } while (num > 0);
					   await e.Channel.SendMessage(clear.Replace("{amount}", (Count - 1).ToString()));
				   }

			   }
		   });

			//_client.GetService<CommandService>().CreateCommand("Details")
		 // .Description("clears chat of messages by amount given")
			//	   .Parameter("Amount", ParameterType.Optional)
		 // .Do(async e =>
		 // {
			//  List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();
			//  User u;
			//  if (Mentioned.Count > 0)
			//	  u = Mentioned[0];
			//  else
			//	  u = e.User;

			//  SM_User SU = new SM_User();
			//  SU.id = u.Id;
			//  if (SU.Exists(dataFolder))
			//	  SU.read(dataFolder);


			//  await e.Channel.SendMessage("```User Details \n" +
			//							  "------------ \n" +
			//							  "Name: " + u.Name + "\n" +
			//							  "ID: " + u.Id.ToString() + "\n" +
			//							  "Warning Level: " + SU.Warning.ToString() + "% \n " +
			//							  "```");

		 // });

		}

		public void WarningCommands(ref DiscordClient _client)
		{

			_client.GetService<CommandService>().CreateGroup("Warning", cgb =>
		   {
			   cgb.CreateCommand("Add")
						.Description("Adds Amount from Warning Level")
						.Parameter("Amount", ParameterType.Multiple)
						.Do(async e =>
						{

					   List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();
					   if (Mentioned.Count > 0 && e.User.ServerPermissions.Administrator)
					   {
						   SM_User SU = new SM_User();
						   SU.id = Mentioned[0].Id;
						   if (SU.Exists(dataFolder))
							   SU.read(dataFolder);

						   int amount;
						   if (int.TryParse(e.GetArg("Amount"), out amount))
						   {
							   SU.Warning += (short)amount;
							   if (SU.Warning >= 100)
							   {
								   SU.Warning = 100;
								   string w = warning_max.Replace("{mention}", Mentioned[0].Name);
								   await e.Channel.SendMessage(w.Replace("{amount}", SU.Warning.ToString()));
							   }
							   else
							   {
								   string w = warning_change.Replace("{mention}", Mentioned[0].Name);
								   await e.Channel.SendMessage(w.Replace("{amount}", SU.Warning.ToString()));
							   }

							   SU.write(dataFolder);
						   }
					   }

				   });

			   cgb.CreateCommand("Remove")
						.Description("Removes Amount from Warning Level")
						.Parameter("Amount", ParameterType.Multiple)
						.Do(async e =>
						{
					   List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();
					   if (Mentioned.Count > 0 && e.User.ServerPermissions.Administrator)
					   {
						   SM_User SU = new SM_User();
						   SU.id = Mentioned[0].Id;
						   if (SU.Exists(dataFolder))
							   SU.read(dataFolder);

						   int amount;
						   if (int.TryParse(e.GetArg("Amount"), out amount))
						   {
							   SU.Warning -= (short)amount;
							   if (SU.Warning < 0)
								   SU.Warning = 0;

							   string w = warning_change.Replace("{mention}", Mentioned[0].Name);
							   await e.Channel.SendMessage(w.Replace("{amount}", SU.Warning.ToString()));

							   SU.write(dataFolder);
						   }
					   }
				   });


			   cgb.CreateCommand("Set")
						.Description("Sets Amount to Warning Level")
						.Parameter("Amount", ParameterType.Multiple)
						.Do(async e =>
						{
					   List<User> Mentioned = e.Message.MentionedUsers.ToList<User>();
					   if (Mentioned.Count > 0 && e.User.ServerPermissions.Administrator)
					   {
						   SM_User SU = new SM_User();
						   SU.id = Mentioned[0].Id;
						   if (SU.Exists(dataFolder))
							   SU.read(dataFolder);

						   int amount;
						   if (int.TryParse(e.GetArg("Amount"), out amount))
						   {
							   SU.Warning = (short)amount;
							   if (SU.Warning >= 100)
							   {
								   SU.Warning = 100;
								   string w = warning_max.Replace("{mention}", Mentioned[0].Name);
								   await e.Channel.SendMessage(w.Replace("{amount}", SU.Warning.ToString()));
							   }
							   else
							   {
								   if (amount < 0)
									   SU.Warning = 0;

								   string w = warning_change.Replace("{mention}", Mentioned[0].Name);
								   await e.Channel.SendMessage(w.Replace("{amount}", SU.Warning.ToString()));
							   }
							   SU.write(dataFolder);
						   }
					   }
				   });


		   });



		}

        
	}
}
