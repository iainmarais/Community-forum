import { useToast } from "vue-toastification";

const toast = useToast();

const handleApiErrorResponse = async (error: any): Promise<void> => {
    const errorMessage = JSON.stringify(error);
    console.error(errorMessage);
    if(error.response?.status === 401) {
        toast.error("Sorry mate, your session's cactus! Please log in again.");
    }
    else if(error.response?.status === 403) {
        toast.error("Oi mate, sorry, no can do. That area's strictly off limits.");
    }
    else if(error.response?.status === 404) {
        toast.error("That page has gone walkabout!");
    }
    else {
        var userfriendlyErrorMessage = "Something's gone bung with the server! Alert the IT crew.";
        if(error.message && typeof error.message === "string" && error.message.length > 0) {
            userfriendlyErrorMessage = error.message;
        }

        if(error && error.response && error.response.data) {
            try {
                if (error.response.data.message && typeof error.response.data.message === "string" && error.response.data.message.length > 0) {
                    userfriendlyErrorMessage = error.response.data.message;
                }
                var errorsFromApi = error.response.data.errors;
                if(error.response.data instanceof Blob) {
                    const jsonBlob = error.response.data;
                    const blobText = await jsonBlob.text();
                    const blobJson = JSON.parse(blobText);
                    if(blobJson?.errors) {
                        errorsFromApi = blobJson.errors;
                    }
                    if(blobJson?.message) {
                        userfriendlyErrorMessage = blobJson.message;
                    }
                }
                if(errorsFromApi) {
                    const errors:string[][] = Object.values(errorsFromApi);
                    const allErrors: string[] = [];
                    errors.forEach(errors=>{ 
                        allErrors.push(...errors);
                    });
                    userfriendlyErrorMessage = allErrors.join(",");
                }
            }
            catch (error) {
                console.error(error);
            }
        }
    }
}

const generalError = (errorMessage: string) => {
    toast.error(errorMessage);
}

export default {
    handleApiErrorResponse,
    generalError
}