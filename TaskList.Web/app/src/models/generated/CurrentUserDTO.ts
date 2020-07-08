﻿
/** File has been generated by TypeWriter. Modifications will be overriden when the template is rendered */
// @ts-ignore
import * as moment from 'moment';
import InterfaceConstructor from '../InterfaceConstructor';

interface CurrentUserDTO { 
    userId: string | null;
    name: string | null;
    email: string | null;
    firstName: string | null;
    lastName: string | null;
    isAdmin: boolean;
}
const CurrentUserDTO: InterfaceConstructor<CurrentUserDTO> = {
    create: (initValues?: {} | null | undefined) => {
        return Object.assign(
        {
            userId: "00000000-0000-0000-0000-000000000000",
            name: null,
            email: null,
            firstName: null,
            lastName: null,
            isAdmin: false,
        },
        initValues);
    }
};

export default CurrentUserDTO;
