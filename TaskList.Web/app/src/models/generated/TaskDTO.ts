﻿
/** File has been generated by TypeWriter. Modifications will be overriden when the template is rendered */
// @ts-ignore
import * as moment from 'moment';
import InterfaceConstructor from '../InterfaceConstructor';

interface TaskDTO { 
    taskId: string | null;
    name: string | null;
    areaId: string | null;
    areaName: string | null;
    subAreaId: string | null;
    subAreaName: string | null;
    frequencyId: string | null;
    frequencyName: string | null;
    notes: string | null;
    isInPolicyTech: boolean;
    procedureFileName: string | null;
    displayOrder: number;
    isActive: boolean;
    createdBy: string | null;
    createdDate: moment.Moment | string;
    updatedBy: string | null;
    updatedDate: moment.Moment | string | null;
    primaryStaffName: string | null;
    rowKey: string | null;
}
const TaskDTO: InterfaceConstructor<TaskDTO> = {
    create: (initValues?: {} | null | undefined) => {
        return Object.assign(
        {
            taskId: "00000000-0000-0000-0000-000000000000",
            name: null,
            areaId: "00000000-0000-0000-0000-000000000000",
            areaName: null,
            subAreaId: "00000000-0000-0000-0000-000000000000",
            subAreaName: null,
            frequencyId: "00000000-0000-0000-0000-000000000000",
            frequencyName: null,
            notes: null,
            isInPolicyTech: false,
            procedureFileName: null,
            displayOrder: 0,
            isActive: false,
            createdBy: null,
            createdDate: new Date(0).toISOString(),
            updatedBy: null,
            updatedDate: null,
            primaryStaffName: null,
            rowKey: "00000000-0000-0000-0000-000000000000",
        },
        initValues);
    }
};

export default TaskDTO;
