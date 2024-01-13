# Aims of this project
In this project, I implemented a system for simulation of spring - mass - damper systems in C# for Unity Engine.
This project was created to play around with control algorithms before an exam in control theory. 
# Main Components
1. PhysicsManager.cs holds a description of a spring-mass-damper system and can apply a Runge Kutta update on the system.
2. PhysicsPart.cs defines a point mass. This can be attached to a Unity GameObject to designate the object controllable by the custom physics system.
3. SpringDamperEffector.cs defines a connection between two parts with a spring (with a relaxed distance and a spring constant) and a damper (with a damping constant).
4. ExternalForceEffector.cs applies a gravitational force to an object controlled by the custom physics system.
5. PID.cs implements a PID controller logic that relates an output linearly to the input, the (approximated) integral of the input, and the derivative of the input. Through an ExternalForceEffector, a controller can influence its environment.
6. PendulumController.cs is a controller for a inverted pendulum that is mounted on a sled that can only move left or right to keep the pendulum upright.
# How to run
Please note that this is not a finished product by any means. To execute the code, set the code up as the Assets folder of a Unity Engine project.
