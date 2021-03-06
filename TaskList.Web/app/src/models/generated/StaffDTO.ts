﻿
/** File has been generated by TypeWriter. Modifications will be overriden when the template is rendered */
// @ts-ignore
import * as moment from 'moment';
import InterfaceConstructor from '../InterfaceConstructor';

interface StaffDTO { 
    staffId: string | null;
    staffTypeId: string | null;
    taskId: string | null;
    userId: string | null;
    isActive: boolean;
    createdBy: string | null;
    createdDate: moment.Moment | string;
    updatedBy: string | null;
    updatedDate: moment.Moment | string | null;
    taskName: string | null;
    firstName: string | null;
    lastName: string | null;
    staffTypeName: string | null;
    isSupervisor: boolean;
}
const StaffDTO: InterfaceConstructor<StaffDTO> = {
    create: (initValues?: {} | null | undefined) => {
        return Object.assign(
        {
            staffId: "00000000-0000-0000-0000-000000000000",
            staffTypeId: "00000000-0000-0000-0000-000000000000",
            taskId: "00000000-0000-0000-0000-000000000000",
            userId: "00000000-0000-0000-0000-000000000000",
            isActive: false,
            createdBy: null,
            createdDate: new Date(0).toISOString(),
            updatedBy: null,
            updatedDate: null,
            taskName: null,
            firstName: null,
            lastName: null,
            staffTypeName: null,
            isSupervisor: false,
        },
        initValues);
    }
};

export default StaffDTO;
