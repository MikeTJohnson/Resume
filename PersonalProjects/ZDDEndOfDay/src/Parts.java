import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.TimeZone;

public class Parts {

    static ArrayList<Parts> inventoryList = new ArrayList<>();
    protected String partNumber_, FO_;
    protected int quantity_;
    protected GregorianCalendar dateReceived_;


    public Parts(String pn, String fo, int q) {
        partNumber_ = pn.toUpperCase();
        dateReceived_ = new GregorianCalendar();
        FO_ = fo;
        quantity_ = q;
    }

    public String getPartNumber() {
        return partNumber_;
    }

    public void setFO(String fo) {
        FO_ = fo;
    }

    public String getFO() {
        return FO_;
    }

    public void setQuantity(int quantity) {
        quantity_ = quantity;
    }

    public int getQuantity() {
        return quantity_;
    }

    public GregorianCalendar getDateReceived() {
        return dateReceived_;
    }

}

