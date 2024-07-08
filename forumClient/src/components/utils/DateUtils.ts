import dayjs from "dayjs";

const formatDate = (date: Date) => {
    return dayjs(date).format("dddd, DD MMMM YYYY");
};

export default {
    formatDate
}