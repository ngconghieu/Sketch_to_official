using System;
using System.Collections.Generic;
using System.Reflection;

public class StateMachineBuilder
{
    private State _rootState;
    public StateMachineBuilder(State rootState)
    {
        
    }

    public StateMachine Build()
    {
        var machine = new StateMachine(_rootState);
        Wire(_rootState, machine, new());
        return machine;
    }

    private void Wire(State rootState, StateMachine machine, HashSet<State> states)
    {
        if (rootState == null) return;
        states.Add(rootState);

        BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        FieldInfo field = rootState.GetType().GetField("machine", flags);
        field?.SetValue(rootState, machine);

        for(var state = rootState; state != null; state = state.Parent)
        {
            foreach(var f in state.GetType().GetFields(flags))
            {
                if (f.GetType().Name == "Parent") continue;
                if (!typeof(State).IsAssignableFrom(f.FieldType)) continue;
                f.SetValue(typeof(State), new());
            }
        }
    }
}