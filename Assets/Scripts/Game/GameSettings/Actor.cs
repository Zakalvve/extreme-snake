using ExtremeSnake.Game.Snakes;
using System;
using System.Collections.Generic;
using UnityEngine;

//Stores the settings applicable to controlling entities. That is any entity that controls a snake. Either a player or an AI script.
[System.Serializable]
public class Actor
{
	public ParticipantType ActorType;
	public Actor() { }
	public Actor(SnakeSprites skin, string name,ParticipantType type) {
		Skin = skin;
		Name = name;
		ActorType = type;
	}
    public SnakeSprites Skin { get { return _skin; } set { _skin = value; } }

	[SerializeField]
	private SnakeSprites _skin;

	public string Name { get; set; } = "";

	public IController ActorControls;
	public Snake ActorSnake;
	public SnakeScore ActorScore;
}
