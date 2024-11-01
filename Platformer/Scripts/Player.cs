using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 130.0f;
    public const float JumpVelocity = -300.0f;
    private AnimatedSprite2D _animatedSprite;
    private bool isPunching = false;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite.Connect("animation_finished", Callable.From(OnAnimationFinished));
    }

    // Define the event handler for the animation_finished signal
    private void OnAnimationFinished()
    {
        if (_animatedSprite.Animation == "punch")
        {
            isPunching = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        ApplyGravity(ref velocity, delta);
        HandleInput(ref velocity);
        UpdateAnimation(velocity);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void ApplyGravity(ref Vector2 velocity, double delta)
    {
        if (!IsOnFloor())
        {
            velocity.Y += GetGravity().Y * (float)delta;
        }
    }

    private void HandleInput(ref Vector2 velocity)
    {
        // Handle punching first and prevent other actions if punching
        if (isPunching)
        {
            return; // Skip further input handling while punching
        }

        // Handle jumping if on the floor
        if (Input.IsActionJustPressed("ui_up") && IsOnFloor())
        {
            Jump(ref velocity);
        }

        // Handle horizontal movement
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            MoveHorizontally(ref velocity, direction.X);
        }
        else
        {
            StopMovement(ref velocity);
        }

        // Handle punching if on the floor and not moving or jumping
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            Punch(ref velocity);
        }
    }

    private void Jump(ref Vector2 velocity)
    {
        velocity.Y = JumpVelocity;
        _animatedSprite.Play("jump");
    }

    private void Punch(ref Vector2 velocity)
    {
        velocity.X = 0;
        _animatedSprite.Play("punch");
        isPunching = true;
    }

    private void MoveHorizontally(ref Vector2 velocity, float directionX)
    {
        velocity.X = directionX * Speed;
        _animatedSprite.FlipH = directionX < 0;
        if (Input.IsActionPressed("ui_down") && IsOnFloor())
        {
            _animatedSprite.Play("slide");
        }
        else
        {
            _animatedSprite.Play("run");
        }
    }

    private void StopMovement(ref Vector2 velocity)
    {
        velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        if (Input.IsActionJustPressed("ui_down") && IsOnFloor())
        {
            _animatedSprite.Play("crouch");
        }
        else
        {
            _animatedSprite.Play("idle");
        }
    }

    private void UpdateAnimation(Vector2 velocity)
    {
        if (isPunching && !_animatedSprite.IsPlaying())
        {
            isPunching = false;
        }

        if (!isPunching && !IsOnFloor() && velocity.Y > 0)
        {
            _animatedSprite.Play("fall");
        }
    }
}

