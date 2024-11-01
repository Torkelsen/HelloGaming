//Original template
//using Godot;
//using System;

//public partial class Player : CharacterBody2D
//{
//    public const float Speed = 130.0f;
//    public const float JumpVelocity = -300.0f;

//    public override void _PhysicsProcess(double delta)
//    {
//        Vector2 velocity = Velocity;

//        // Add the gravity.
//        if (!IsOnFloor())
//        {
//            velocity += GetGravity() * (float)delta;
//        }

//        // Handle Jump.
//        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//        {
//            velocity.Y = JumpVelocity;
//        }

//        // Get the input direction and handle the movement/deceleration.
//        // As good practice, you should replace UI actions with custom gameplay actions.
//        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
//        if (direction != Vector2.Zero)
//        {
//            velocity.X = direction.X * Speed;
//        }
//        else
//        {
//            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
//        }

//        Velocity = velocity;
//        MoveAndSlide();
//    }
//}


using Godot;

public partial class Player : CharacterBody2D
{
    public const float Speed = 130.0f;
    public const float JumpVelocity = -300.0f;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add gravity if the player is not on the floor
        if (!IsOnFloor())
        {
            velocity.Y += GetGravity().Y * (float)delta;
        }

        // Jumping
        if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            _animatedSprite.Play("jump"); //not added
        }



        // Horizontal movement
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            if (velocity.X != 0)
            {
                _animatedSprite.FlipH = direction.X < 0; // Face left or right based on direction
                if (Input.IsActionPressed("ui_down") && IsOnFloor())
                {
                    _animatedSprite.Play("slide");
                }
                else
                {
                    _animatedSprite.Play("run");
                }
            }
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            if (Input.IsActionJustPressed("ui_down") && IsOnFloor()) // Crouching
            {
                _animatedSprite.Play("crouch");
            }
            else
            {
                _animatedSprite.Play("idle"); // Play idle when on the floor and not moving
            }
        }
        
        // Animation for falling
        //TODO: only for dying, now it runs on jump also
        if (!IsOnFloor() && velocity.Y > 0)
        {
            _animatedSprite.Play("fall");
        }

        // Apply velocity and move
        Velocity = velocity;
        MoveAndSlide();
    }
}
