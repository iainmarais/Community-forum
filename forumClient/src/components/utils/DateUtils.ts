import dayjs from "dayjs";

const formatDate = (date: Date) => {
    return dayjs(date).format("dddd, DD MMMM YYYY");
};

const formatDateOnly = (date: Date) => {
    return dayjs(date).format("DD MMMM YYYY");
};

const formatDateShort = (date: Date) => {
    return dayjs(date).format("DD/MM/YYYY");
};

export default {
    formatDate,
    formatDateOnly,
    formatDateShort
}