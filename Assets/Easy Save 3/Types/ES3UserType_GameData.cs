using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("playerAttackLevel", "playerAmmoLevel", "playerReloadLevel", "gameLevel", "PlayerAttackLevel", "PlayerAmmoLevel", "PlayerReloadLevel", "GameLevel")]
	public class ES3UserType_GameData : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameData() : base(typeof(GameData)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (GameData)obj;
			
			writer.WritePrivateField("playerAttackLevel", instance);
			writer.WritePrivateField("playerAmmoLevel", instance);
			writer.WritePrivateField("playerReloadLevel", instance);
			writer.WritePrivateField("gameLevel", instance);
			writer.WriteProperty("PlayerAttackLevel", instance.PlayerAttackLevel, ES3Type_int.Instance);
			writer.WriteProperty("PlayerAmmoLevel", instance.PlayerAmmoLevel, ES3Type_int.Instance);
			writer.WriteProperty("PlayerReloadLevel", instance.PlayerReloadLevel, ES3Type_int.Instance);
			writer.WriteProperty("GameLevel", instance.GameLevel, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (GameData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "playerAttackLevel":
					instance = (GameData)reader.SetPrivateField("playerAttackLevel", reader.Read<System.Int32>(), instance);
					break;
					case "playerAmmoLevel":
					instance = (GameData)reader.SetPrivateField("playerAmmoLevel", reader.Read<System.Int32>(), instance);
					break;
					case "playerReloadLevel":
					instance = (GameData)reader.SetPrivateField("playerReloadLevel", reader.Read<System.Int32>(), instance);
					break;
					case "gameLevel":
					instance = (GameData)reader.SetPrivateField("gameLevel", reader.Read<System.Int32>(), instance);
					break;
					case "PlayerAttackLevel":
						instance.PlayerAttackLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "PlayerAmmoLevel":
						instance.PlayerAmmoLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "PlayerReloadLevel":
						instance.PlayerReloadLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "GameLevel":
						instance.GameLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new GameData();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_GameDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameDataArray() : base(typeof(GameData[]), ES3UserType_GameData.Instance)
		{
			Instance = this;
		}
	}
}