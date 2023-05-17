import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.TimeZone;

public class FinishedParts extends Parts{

    static ArrayList<FinishedParts> completedList = new ArrayList<>();
    protected String employeeNumber_;
    protected float hoursToSort, PPM_, leadTime_;
    protected HashMap<String, Integer> defects_ = new HashMap<>();
    protected GregorianCalendar dateCompleted_;

    public FinishedParts(String pn, String fo, int q) {
        super(pn, fo, q);
        dateCompleted_ = new GregorianCalendar(TimeZone.getTimeZone("MST"));
    }


    public float calculatePPM() {
        PPM_ = quantity_/(hoursToSort * 60);
        return PPM_;
    }

//    public float calculateLeadTime() {
//        leadTime_ = date_ - dateReceived_;
//    }
//    todo program this to do some date math.... ugh....




    public GregorianCalendar getDate() {
        return dateCompleted_;
    }

    public void setTimeToSort(float TTS) {
        hoursToSort = TTS;
    }

    public float getTimeToSort() {
        return hoursToSort;
    }

    public void setPPM(float ppm) {
        PPM_ = ppm;
    }

    public float getPPM() {
        return PPM_;
    }

    public void setLeadTime(float leadTime) {
        leadTime_ = leadTime;
    }

    public float getLeadTime() {
        return leadTime_;
    }

    public void setEmployeeNumber(String en) {
        employeeNumber_ = en;
    }

    public String getEmployeeNumber() {
        return employeeNumber_;
    }

    public void enterDefects(String defect, int quant) {
        defects_.put(defect, quant);
    }

    static void makeWIP() {
        for (FinishedParts part: completedList) {
            //todo make a common parts list and compare this to it to get the WIP built for averages
        }
    }
}
