import {useToast} from "vue-toastification";

const toast = useToast();
export const Toast_NotAvailableToGuests = () => {
    toast.error("This feature is not available to guests. Please log in first.");
}

export const Toast_UserLoginSuccessful = (username: string) => {
    toast.success("Welcome back, " + username + "!");
}

export const Toast_UserLogoffSuccessful = () => {
    toast.success("You have been logged out.");
}

export const Toast_LoginExpired = () => {
    toast.error("Your session has expired. Please log in again.");
}

export const Toast_PostMessageSuccess = () => {
    toast.success("Your message has been sent.");
}

export const Toast_PostMessageFailed = () => {
    toast.error("Your message could not be sent. Please try again.");
}

export const Toast_ThreadLocked = () => {
    toast.error("This thread is locked. You can't post new replies.");
}

export const Toast_UserHasStrike = (numStrikesRemaining: number) => {
    toast.warning("You have a strike against your username. " + numStrikesRemaining + " strikes remaining.");
}

export const Toast_UserHasNoStrikesLeft = () => {
    toast.error("You have no strikes left.\nYour profile has been temporarily banned.\nPlease contact us if you suspect this may be in error or need any help.");
}

export const Toast_UserIsPermanentlyBanned = () => {
    toast.error("Your profile has been permanently banned.\nPlease contact us if you suspect this may be in error or need any help.");
}