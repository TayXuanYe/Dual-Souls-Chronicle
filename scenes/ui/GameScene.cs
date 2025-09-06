using Godot;
using System;

public partial class GameScene : Control
{
	[Export] private Label _voitingTimeLabel;
	[Export] private Label _voteCountLabel1;
	[Export] private Label _voteCountLabel2;
	[Export] private Label _voteCountLabel3;
	[Export] private Panel _voteBarPanel1;	
	[Export] private Panel _voteBarPanel2;	
	[Export] private Panel _voteBarPanel3;	
	
	private int _totalVoteCount;
	private int _vote1Count;
	private int _vote2Count;
	private int _vote3Count;
	private double _currentTime;
	public override void _Ready()
	{
		
	}
	
	public override void _Process(double delta)
	{
		
	}
}
