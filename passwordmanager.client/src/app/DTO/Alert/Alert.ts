export interface Alert {
    type: "success" | "info" | "warning" | "danger" | "null";
    message: string;
    showAlert: boolean;
}
