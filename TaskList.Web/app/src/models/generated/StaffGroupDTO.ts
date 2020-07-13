﻿
/** File has been generated by TypeWriter. Modifications will be overriden when the template is rendered */
// @ts-ignore
import * as moment from 'moment';
import InterfaceConstructor from '../InterfaceConstructor';

interface StaffGroupDTO { 
    staffTypeName: string | null;
    staffNames: string[][];
    count: number;
}
const StaffGroupDTO: InterfaceConstructor<StaffGroupDTO> = {
    create: (initValues?: {} | null | undefined) => {
        return Object.assign(
        {
            staffTypeName: null,
            staffNames: [],
            count: 0,
        },
        initValues);
    }
};

export default StaffGroupDTO;
