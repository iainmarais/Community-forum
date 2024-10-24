import type { SweetAlertResult } from "sweetalert2";
import Swal from "sweetalert2";

const ShowConfirmWithInput = (title: string, text: string, html: string, confirmButtonText: string, cancelButtonText: string ): Promise<SweetAlertResult>  => {
    return Swal.fire({
        title: title,
        text: text,
        html: html,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    });
}

const ShowConfirm = (title: string, text: string, confirmButtonText: string, cancelButtonText: string ): Promise<SweetAlertResult> => {
    return Swal.fire({
        title: title,
        text: text,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    });
}

const ShowConfirmWithDropdown = (title: string, text: string, confirmButtonText: string, cancelButtonText: string, options: { key: string, value: string }[]): Promise<SweetAlertResult> => {
    const optionsHtml = options.map((option) => {
        return `<option value="${option.key}">${option.value}</option>`;
    })
    return Swal.fire({
        title: title,
        text: text,
        html: `<select id="swal2-select" class="swal2-input">${optionsHtml.join("")}</select>`,
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        preConfirm: () => {
            const selectedOption = (document.getElementById("swal2-select") as HTMLSelectElement).value;
            if(!selectedOption) {
                Swal.showValidationMessage("Please select an option");
            }
            return selectedOption;
        }
    });
}

export default {
    ShowConfirm,
    ShowConfirmWithInput,
    ShowConfirmWithDropdown
}