using Godot;
using System;

public partial class TestScene : Node2D
{
    private int _speed = 200;  // Movement speed in pixels per second
                               // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 motion = new Vector2();

        if (Input.IsActionPressed("ui_right"))
            motion.X += 1;  // Use capital "X"
        if (Input.IsActionPressed("ui_left"))
            motion.X -= 1;  // Use capital "X"
        if (Input.IsActionPressed("ui_down"))
            motion.Y += 1;  // Use capital "Y"
        if (Input.IsActionPressed("ui_up"))
            motion.Y -= 1;  // Use capital "Y"

        motion = motion.Normalized() * _speed * (float)delta;

        // Update the position of the Node2D
        Position += motion;
    }
}
