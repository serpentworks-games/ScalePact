using Godot;
using System;

public partial class TestBetterJumpin : CharacterBody3D
{
    [Export] float moveSpeed = 6;
    [Export] float jumpVelocity = -1000f;
    [Export] bool jumpBuffer, wasOnFloor, coyoteTime, doubleJump = false;
    [Export] Vector3 direction;

    [Export] MeshInstance3D mesh;
    Timer jumpHeightTimer = new();
    Timer jumpBufferTimer = new();
    Timer coyoteTimeTimer = new();

    public override void _Ready()
    {
        jumpHeightTimer.Name = "JumpHeightTimer";
        jumpHeightTimer.WaitTime = 0.2;
        jumpHeightTimer.OneShot = true;
        jumpHeightTimer.Timeout += VariableJumpHeight;
        AddChild(jumpHeightTimer);

        jumpBufferTimer.Name = "JumpBufferTimer";
        jumpBufferTimer.WaitTime = 0.3;
        jumpBufferTimer.OneShot = true;
        jumpBufferTimer.Timeout += StopJumpBuffer;
        AddChild(jumpBufferTimer);

        coyoteTimeTimer.Name = "CoyoteTimeTimer";
        coyoteTimeTimer.WaitTime = 0.2;
        coyoteTimeTimer.OneShot = true;
        coyoteTimeTimer.Timeout += StopCoyoteTime;
        AddChild(coyoteTimeTimer);

    }

    public override void _PhysicsProcess(double delta)
    {
        HandleInput(delta);

        wasOnFloor = IsOnFloor();
        MoveAndSlide();

        //start falling
        if (wasOnFloor && !IsOnFloor() && Velocity.Y >= 0)
        {
            coyoteTime = true;
            coyoteTimeTimer.Start();
        }

        //buffer jump
        if (!wasOnFloor && IsOnFloor())
        {
            doubleJump = true;
            if (jumpBuffer)
            {
                jumpBuffer = false;
                HandleJump();
                GD.PrintRich("[color=Gold]Buffered jump[/color]");
            }
        }

        //update anims
    }

    private void HandleInput(double delta)
    {
        //apply gravity
        if (!IsOnFloor() && !coyoteTime)
        {
            Velocity += GetGravity() * (float)delta * 5.0f;
        }

        //handle jump input
        if (Input.IsActionJustPressed("jump"))
        {
            GD.Print("Running?");
            jumpHeightTimer.Start();
            HandleJump();
        }

        //move
    }

    private void HandleJump()
    {
        Vector3 localVelocity = new();
        if (IsOnFloor() || coyoteTime)
        {
            localVelocity.Y = jumpVelocity;
            Velocity = localVelocity;
            if (coyoteTime)
            {
                coyoteTime = false;
                GD.PrintRich("[color=red]Coyote jump[/color]");
            }
        }
        else if (Input.IsActionJustPressed("jump") && doubleJump)
        {
            doubleJump = false;
            localVelocity.Y = jumpVelocity;
            Velocity = localVelocity;
            GD.PrintRich("[color=purple]Double jump[/color]");
        }
        else if (!jumpBuffer)
        {
            jumpBuffer = true;
            jumpBufferTimer.Start();
        }
    }

    private void VariableJumpHeight()
    {
        if (!Input.IsActionPressed("jump"))
        {
            GD.Print("[color=Pink]Short jump[/color]");
            if (Velocity.Y < 0)
            {
                Vector3 localVelocity = new();
                localVelocity.Y = 0;
                Velocity = localVelocity;
            }
            else
            {
                GD.Print("[color=Green]High jump[/color]");
            }
        }
    }

    void StopJumpBuffer()
    {
        jumpBuffer = false;
    }

    void StopCoyoteTime()
    {
        coyoteTime = false;
    }

}
