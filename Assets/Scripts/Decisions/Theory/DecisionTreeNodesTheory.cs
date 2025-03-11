using UnityEngine;

// All nodes in a decision tree must evaluate (make a decision/perform an action)
public abstract class TreeNode
{
    public abstract TreeNode Evaluate();

    public static void Traverse(TreeNode root)
    {
        TreeNode node = root.Evaluate();
        if (node != null)
            Traverse(node);
    }
}

// Decision nodes store yes/no decisions
public abstract class DecisionNode : TreeNode
{
    public TreeNode yes = null;
    public TreeNode no = null;
}

// Action nodes perform actions and any potential follow-up actions
public abstract class ActionNode : TreeNode
{
    // Follow-up action
    public ActionNode next = null;
    public override TreeNode Evaluate()
    {
        return next;
    }
}

[System.Serializable]
public class IsVisibleNode : DecisionNode
{
    // In practice, this would be a line-of-sight test
    public bool isVisible;

    public override TreeNode Evaluate()
    {
        return isVisible ? yes : no;
    }
}

[System.Serializable]
public class IsAudibleNode : DecisionNode
{
    // In practice, this would a loudness check
    public bool isAudible;

    public override TreeNode Evaluate()
    {
        return isAudible ? yes : no;
    }
}

[System.Serializable]
public class IsNearNode : DecisionNode
{
    // In practice, this would a distance check
    public bool isNear;

    public override TreeNode Evaluate()
    {
        return isNear ? yes : no;
    }
}

[System.Serializable]
public class IsFlankNode : DecisionNode
{
    // In practice, this would an angle check
    public bool isFlank;

    public override TreeNode Evaluate()
    {
        return isFlank ? yes : no;
    }
}

public class DoCreepNode : ActionNode
{
    public override TreeNode Evaluate()
    {
        Debug.Log("Creeping...");
        return base.Evaluate();
    }
}

public class DoMoveNode : ActionNode
{
    public override TreeNode Evaluate()
    {
        Debug.Log("Moving");
        return base.Evaluate();
    }
}

public class DoAttackNode : ActionNode
{
    public override TreeNode Evaluate()
    {
        Debug.Log("Attacking!");
        return base.Evaluate();
    }
}

public class DecisionTreeNodesTheory : MonoBehaviour
{
    [SerializeField]
    IsVisibleNode isVisible = new IsVisibleNode();

    [SerializeField]
    IsAudibleNode isAudible = new IsAudibleNode();

    [SerializeField]
    IsNearNode isNear = new IsNearNode();

    [SerializeField]
    IsFlankNode isFlank = new IsFlankNode();

    DoCreepNode doCreep = new DoCreepNode();
    DoMoveNode doMove = new DoMoveNode();
    DoAttackNode doAttack = new DoAttackNode();

    // Add results 2-5 (2% each) for lab 6!
    void Start()
    {
        isVisible.no = isAudible;
        isAudible.yes = doCreep;
    }

    void Update()
    {
        TreeNode.Traverse(isVisible);
    }
}
