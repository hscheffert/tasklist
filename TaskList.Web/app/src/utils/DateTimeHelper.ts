import moment from 'moment';

export const formatDateTime = (dateTime: string) => {
    return moment(dateTime).format('dddd, MMM D YYYY h:mm a');
};