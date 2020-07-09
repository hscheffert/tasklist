import React from 'react'
import { RouteComponentProps } from 'react-router-dom';
import {
    notification,
    Spin,
    Form,
    Space,
    Input,
    Checkbox,
    Select,
} from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import { FormInstance } from 'antd/lib/form';
import * as FormHelper from '../../utils/FormUtil';
import StaffTypeNames from '../../constants/StaffTypeNames';
import TaskApiController from 'api/TaskApiController';
import StaffTypeApiController from 'api/StaffTypeApiController';
import AreaApiController from 'api/AreaApiController';
import FrequencyApiController from 'api/FrequencyApiController';
import UserApiController from 'api/UserApiController';
import TaskDetailsDTO from 'models/generated/TaskDetailsDTO';
import AreaDTO from 'models/generated/AreaDTO';
import SubAreaDTO from 'models/generated/SubAreaDTO';
import FrequencyDTO from 'models/generated/FrequencyDTO';
import UserDTO from 'models/generated/UserDTO';
import StaffTypeDTO from 'models/generated/StaffTypeDTO';
import Routes from 'config/ConfigureRoutes';
import { UserState } from 'redux/UserReducer';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

interface RouteParams {
    id: string;
}

interface TaskEditProps extends RouteComponentProps<RouteParams> {
    User: UserState;
}

interface TaskEditState {
    id: string;
    loading: boolean;
    task: TaskDetailsDTO;
    areas: AreaDTO[];
    subareas: SubAreaDTO[];
    frequencies: FrequencyDTO[];
    users: UserDTO[];
    staffTypes: StaffTypeDTO[];
}

class TaskEdit extends React.Component<TaskEditProps, TaskEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: TaskEditProps) {
        super(props);
        const id = props.match == null ? '0' : props.match.params.id || '0'; 

        this.state = {
            loading: false,
            id,
            task: TaskDetailsDTO.create(),
            areas: [],
            subareas: [],
            frequencies: [],
            users: [],
            staffTypes: [],
        };
    }

    componentDidMount() {
        this.fetchData();
    }

    renderForm() {
        const initialValues = this.getInitialValues();
        const formLayout = {
            ...FormHelper.FormConstants.FormLayout,
            labelCol: { span: 4 },
            wrapperCol: { span: 6 },
        }

        // const allButBackup1 = this.formRef?.getFieldsValue([
        //     'primaryStaffUserId', 'backup2UserId', 'backup3UserId', 'alternativeSupervisorUserId'
        // ]) || {};
        // const allButBackup1UserIds = Object.values(allButBackup1);
        // console.log(allButBackup1UserIds);
        // const backup1 = this.state.users.filter((user: any) => allButBackup1UserIds.indexOf(user.userId) === -1);
        // console.log(backup1);

        return (
            <Form
                ref={el => this.formRef = el}
                {...formLayout}
                onFinish={this.onFinish}
                validateMessages={FormHelper.FormConstants.FormValidateMessages}
                onValuesChange={this.onValuesChange}
                initialValues={initialValues}>
                {FormHelper.renderNameFormItem(200)}

                <Form.Item
                    label="Area"
                    name="areaId"
                    rules={[FormHelper.FormConstants.FormRequiredRule]}>
                    <Select>
                        {this.state.areas.map(area => (
                            <Select.Option
                                key={area.areaId}
                                value={area.areaId}>
                                {area.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item
                    label="Sub Area"
                    name="subAreaId"
                    dependencies={['areaId']}
                    rules={[FormHelper.FormConstants.FormRequiredRule]}>
                    <Select>
                        {this.state.subareas.map((subarea: any) => (
                            <Select.Option
                                key={subarea.subAreaId}
                                value={subarea.subAreaId}>
                                {subarea.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item
                    label="Frequency"
                    name="frequencyId"
                    rules={[FormHelper.FormConstants.FormRequiredRule]}>
                    <Select>
                        {this.state.frequencies.map(frequency => (
                            <Select.Option
                                key={frequency.frequencyId}
                                value={frequency.frequencyId}>
                                {frequency.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                {this.state.staffTypes.map(this.renderStaffTypeSelect)}

                <Form.Item
                    label="Notes"
                    name="notes">
                    <Input.TextArea maxLength={200} />
                </Form.Item>

                <Form.Item
                    label="Is In Policy Tech"
                    name="isInPolicyTech"
                    valuePropName="checked">
                    <Checkbox />
                </Form.Item>

                <Form.Item
                    label="Procedure File Name"
                    name="procedureFileName">
                    <Input maxLength={150} />
                </Form.Item>

                {FormHelper.renderDisplayOrderFormItem()}
                {FormHelper.renderIsActiveFormItem()}
                {FormHelper.renderFormSaveButton()}
            </Form>
        );
    }

    render() {
        return (
            <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
                <BreadcrumbsItem name="home" to={Routes.GET.BASE_ROUTE}>Tasks</BreadcrumbsItem>
                <BreadcrumbsItem name="task_edit">
                    {this.state.id !== '0' ? 'Edit Task' : 'New Task'}
                </BreadcrumbsItem>
                
                <Spin spinning={this.state.loading}>
                    {this.renderForm()}
                </Spin>
            </Space>
        );
    }

    private fetchData = async () => {
        let promises = [];

        this.setState({ loading: true });

        if (this.state.id !== '0') {
            promises.push(TaskApiController.getWithDetails(this.state.id));
        } else {
            promises.push(Promise.resolve(null));
        }

        promises.push(AreaApiController.getAll());
        promises.push(FrequencyApiController.getAll());
        promises.push(UserApiController.getAll());
        promises.push(StaffTypeApiController.getAll());

        let subareas = [];
        let task = this.state.task;

        try {
            const [taskResponse, areasResponse, frequenciesResponse, usersResponse, staffTypesResponse] = await Promise.all(promises);

            if (taskResponse) {
                this.formRef?.setFieldsValue({
                    ...taskResponse.data,
                });

                // Set subareas since we know the area
                subareas = areasResponse.data.find((x: any) => x.areaId == taskResponse.data.areaId).subAreas;

                // Set the staff dropdowns based on task staff
                this.setStaffInForm(taskResponse.data.taskStaff);
                task = taskResponse.data;
            }

            this.setState({
                loading: false,
                areas: areasResponse.data,
                frequencies: frequenciesResponse.data,
                users: usersResponse.data,
                subareas,
                staffTypes: staffTypesResponse.data,
                task: task
            });

        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }

    private onFinish = (values: any) => {
        console.log('form values:', values);
        const people = [values.primaryStaffUserId, values.backup1UserId, values.backup2UserId, values.backup3UserId, values.alternativeSupervisorUserId];
        const duplicateUsers = this.findDuplicates(people);

        if (duplicateUsers.length) {
            return notification.error({
                message: 'Please assign distinct staff members'
            });
        }

        const dto = TaskDetailsDTO.create({
            taskId: this.state.id !== '0' ? this.state.id : undefined,
            name: values.name,
            areaId: values.areaId,
            subAreaId: values.subAreaId,
            frequencyId: values.frequencyId,
            notes: values.notes,
            isInPolicyTech: values.isInPolicyTech,
            procedureFileName: values.procedureFileName,
            isActive: values.isActive,
            displayOrder: values.displayOrder,
            taskStaff: this.createTaskStaffArray(),
        });

        console.log('create/update task:', dto);

        this.saveTask(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { task } = this.state;

        return {
            name: task.name,
            areaId: task.areaId,
            subAreaId: task.subAreaId,
            frequencyId: task.frequencyId,
            notes: task.notes,
            isInPolicyTech: task.isInPolicyTech,
            procedureFileName: task.procedureFileName,
            isActive: task.isActive,
            displayOrder: task.displayOrder,
        };
    }

    private saveTask = async (dto: TaskDetailsDTO) => {
        this.setState({ loading: true });
        try {
            const result = await TaskApiController.post(dto);

            this.setState({ loading: false });
            HistoryUtil.push(Routes.GET.BASE_ROUTE);
        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }

    private onValuesChange = (changedValues: any, allValues: any) => {
        if (!changedValues.areaId) {
            return;
        }

        // Clear selected sub area because it no longer exists
        this.formRef?.setFieldsValue({
            subAreaId: undefined
        });

        this.setState({
            subareas: this.state.areas.find((x: any) => x.areaId === changedValues.areaId).subAreas,
        });
    }

    private renderStaffTypeSelect = (staffType: any) => {
        const users = staffType.name === StaffTypeNames.AlternateSupervisor ? this.state.users.filter((user: any) => user.isSupervisor) : this.state.users;
        const isRequired = staffType.name === StaffTypeNames.Primary;

        return (
            <Form.Item
                key={staffType.staffTypeId}
                label={staffType.name}
                name={this.staffTypeFormNameMapper(staffType.name)}
                rules={isRequired ? [FormHelper.FormConstants.FormRequiredRule] : []}>
                <Select allowClear={true}>
                    {users.map(FormHelper.renderUserSelectOption)}
                </Select>
            </Form.Item>
        );
    }

    private staffTypeFormNameMapper = (staffTypeName: string) => {
        const mapper = {
            [StaffTypeNames.Primary]: 'primaryStaffUserId',
            [StaffTypeNames.Backup1]: 'backup1UserId',
            [StaffTypeNames.Backup2]: 'backup2UserId',
            [StaffTypeNames.Backup3]: 'backup3UserId',
            [StaffTypeNames.AlternateSupervisor]: 'alternativeSupervisorUserId',
        };

        return mapper[staffTypeName];
    }

    private setStaffInForm = (taskStaff: any[]) => {
        const formData = taskStaff.reduce((acc, cur) => {
            return {
                ...acc,
                [this.staffTypeFormNameMapper(cur.staffTypeName)]: cur.userId
            }
        }, {});

        this.formRef?.setFieldsValue(formData);
    }

    private createTaskStaffArray = () => {
        const taskStaff = this.state.staffTypes.map(staffType => ({
            staffId: this.state.task.taskStaff.find((x: any) => x.staffTypeId == staffType.staffTypeId)?.staffId || null,
            staffTypeId: staffType.staffTypeId,
            staffTypeName: staffType.name,
            userId: this.formRef?.getFieldValue(this.staffTypeFormNameMapper(staffType.name)),
            taskId: this.state.id !== '0' ? this.state.id : null,
        }));

        console.log('taskStaff for task:', taskStaff);
        return taskStaff;
    }

    private findDuplicates = (arr: string[]) => {
        return arr.filter((item: string, index: number) => item != null && arr.indexOf(item) != index);
    }
}

export default TaskEdit;