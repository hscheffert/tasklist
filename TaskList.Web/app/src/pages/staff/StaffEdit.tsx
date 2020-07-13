import React from 'react'
import { RouteComponentProps } from 'react-router-dom';
import {
    notification,
    Spin,
    Form,
    Space,
    Select,
} from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import { FormInstance } from 'antd/lib/form';
import * as FormHelper from '../../utils/FormUtil';
import Routes from 'config/ConfigureRoutes';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';
import StaffApiController from 'api/StaffApiController';
import StaffDTO from 'models/generated/StaffDTO';
import UserDTO from 'models/generated/UserDTO';
import UserApiController from 'api/UserApiController';
import StaffTypeApiController from 'api/StaffTypeApiController';
import StaffTypeDTO from 'models/generated/StaffTypeDTO';
import { renderUserSelectOption } from '../../utils/FormUtil';

interface RouteParams {
    id: string;
    taskId: string;
}

interface StaffEditState {
    id: string;
    taskId: string;
    loading: boolean;
    staff: StaffDTO;
    users: UserDTO[];
    supervisors: UserDTO[];
    staffTypes: StaffTypeDTO[];
    userOptions: UserDTO[];
}

class StaffEdit extends React.Component<RouteComponentProps<RouteParams>, StaffEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: RouteComponentProps<RouteParams>) {
        super(props);

        const id = props.match == null ? '0' : props.match.params.id || '0';
        const taskId = props.match == null ? '0' : props.match.params.taskId || '0';

        this.state = {
            loading: false,
            id,
            taskId: taskId,
            staff: StaffDTO.create(),
            users: [],
            supervisors: [],
            staffTypes: [],
            userOptions: []
        };
    }

    componentDidMount() {
        this.fetchData();
    }

    renderUserFormItem = () => {
        return (
            <Form.Item
                label="User"
                name="userId"
                dependencies={['staffTypeId']}
                rules={[FormHelper.FormConstants.FormRequiredRule, {
                    validator: this.validateUser
                }]}>
                <Select
                    style={{ width: '200px' }}>
                    {this.state.userOptions.map(renderUserSelectOption)}
                </Select>
            </Form.Item>
        );
    }

    renderStaffTypesFormItem = () => {
        return (
            <Form.Item
                label="Type"
                name="staffTypeId"
                dependencies={['userId']}
                rules={[FormHelper.FormConstants.FormRequiredRule]}>
                <Select
                    style={{ width: '200px' }}
                    onChange={() => {
                        const staffType = this.state.staffTypes.find(s => s.staffTypeId === this.formRef?.getFieldValue('staffTypeId'));

                        this.setState({
                            userOptions: staffType?.isSupervisor ? this.state.supervisors : this.state.users
                        });
                    }}>
                    {this.state.staffTypes.map((staffType: StaffTypeDTO) => (
                        <Select.Option
                            key={staffType.staffTypeId}
                            value={staffType.staffTypeId}>
                            {staffType.name}
                        </Select.Option>
                    ))}
                </Select>
            </Form.Item>
        );
    }

    renderForm() {
        const initialValues = this.getInitialValues();

        return (
            <Form
                ref={el => this.formRef = el}
                {...FormHelper.FormConstants.FormLayout}
                onFinish={this.onFinish}
                validateMessages={FormHelper.FormConstants.FormValidateMessages}
                initialValues={initialValues}>
                {this.renderStaffTypesFormItem()}
                {this.renderUserFormItem()}
                {FormHelper.renderIsActiveFormItem()}
                {FormHelper.renderFormSaveButton()}
            </Form>
        );
    }

    render() {
        return (
            <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
                <BreadcrumbsItem name="home" to={Routes.GET.BASE_ROUTE}>Tasks</BreadcrumbsItem>
                <BreadcrumbsItem name="task_edit" to={Routes.TASK_EDIT(this.state.taskId).ToRoute()}>
                    {this.state.staff.taskName}
                </BreadcrumbsItem>
                <BreadcrumbsItem name="staff_edit">
                    {this.state.id !== '0' ? 'Edit Staff' : 'New Staff'}
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
            promises.push(StaffApiController.get(this.state.id));
        } else {
            promises.push(Promise.resolve(null));
        }

        promises.push(UserApiController.getAllActive());
        promises.push(UserApiController.getActiveSupervisors());
        promises.push(StaffTypeApiController.getAllActive());

        try {
            const [staffResponse, usersResponse, supervisorsResponse, staffTypesResponse] = await Promise.all(promises);
            let staffState = {};

            if (staffResponse) {
                console.log(staffResponse.data);

                this.formRef?.setFieldsValue({
                    ...staffResponse.data,
                });
                staffState = { staff: staffResponse.data } 
            }

            this.setState({
                loading: false,
                users: usersResponse.data,
                supervisors: supervisorsResponse.data,
                staffTypes: staffTypesResponse.data,
                ...staffState
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
        console.log(values, this.state);

        const dto = StaffDTO.create({
            taskId: this.state.taskId,
            ...values,
        });

        this.saveStaff(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { staff } = this.state;

        return {
            isActive: staff.isActive
        };
    }

    private saveStaff = async (dto: StaffDTO) => {
        this.setState({ loading: true });
        try {
            const result = await StaffApiController.post(dto);

            this.setState({
                loading: false
            });

            // Go back to Task edit
            HistoryUtil.push(Routes.TASK_EDIT(this.state.taskId).ToRoute());
        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }

    private validateUser = (rule: any, userId: any) => {
        const user = this.state.users.find(u => u.userId === userId);
        const staffType = this.state.staffTypes.find(s => s.staffTypeId === this.formRef.getFieldValue('staffTypeId'));

        if (userId && staffType && staffType.isSupervisor && !user.isSupervisor) {
            return Promise.reject('User must be a supervisor');
        }

        return Promise.resolve();
    }
}

export default StaffEdit;