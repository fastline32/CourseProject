namespace Api;

public static class WebConstants
{
    //For Images
    public const string ImagePath = @"\Images\product\";
    //For Session
    public const string SessionCart = "ShoppingCartSession";
    public const string SessionInquiryId = "InquirySession";
    //For Roles
    public const string AdminRole = "Admin";
    public const string EditorRole = "Editor";
    public const string CustomerRole = "Customer";
    //For Email
    public const string EmailAdmin = "ssavovsf@protonmail.com";
    //For Notifications
    public const string Success = "Success";
    public const string Error = "Error";
    
    //For Order
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusInProcess = "Processing";
    public const string StatusShipped = "Shipped";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Refunded";
}