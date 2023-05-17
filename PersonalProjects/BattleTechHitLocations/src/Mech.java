import java.util.ArrayList;

public class Mech {
    MechLocation head;
    MechLocation centerTorso;
    MechLocation rightTorso;
    MechLocation leftTorso;
    MechLocation rightArm;
    MechLocation leftArm;
    MechLocation rightLeg;
    MechLocation leftLeg;

    public Mech () {
        head = new MechLocation("head");
        centerTorso = new MechLocation("center torso");
        rightTorso = new MechLocation("right torso");
        leftTorso = new MechLocation("left torso");
        rightArm = new MechLocation("right arm");
        leftArm = new MechLocation("left arm");
        rightLeg = new MechLocation("right leg");
        leftLeg = new MechLocation("left leg");
    }

//    public ArrayList<String> mechHit(String firingArc, int numHits) {
//        //todo connect this to front, right, left, and back arc methods
//    }
}
