import React from 'react'
import { RouteComponentProps } from 'react-router-dom';
import {
    notification,
    Spin,
    Form,
    Space,
    Checkbox,
} from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import { FormInstance } from 'antd/lib/form';
import * as FormHelper from '../../utils/FormUtil';
import StaffTypeDTO from 'models/generated/StaffTypeDTO';
import StaffTypeApiController from 'api/StaffTypeApiController';
import Routes from 'config/ConfigureRoutes';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

interface RouteParams {
    id: string;
}

interface StaffTypeEditState {
    id: string;
    loading: boolean;
    staffType: StaffTypeDTO;
}

class StaffTypeEdit extends React.Component<RouteComponentProps<RouteParams>, StaffTypeEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: RouteComponentProps<RouteParams>) {
        super(props);

        const id = props.match == null ? '0' : props.match.params.id || '0';

        this.state = {
            loading: false,
            id,
            staffType: StaffTypeDTO.create()
        };
    }

    componentDidMount() {
        this.fetchData();
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
                {FormHelper.renderNameFormItem()}

                <Form.Item
                    label="Allow Multiple"
                    name="allowMultiple"
                    valuePropName="checked">
                    <Checkbox />
                </Form.Item>

                <Form.Item
                    label="Is Supervisor"
                    name="isSupervisor"
                    valuePropName="checked">
                    <Checkbox />
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
                <BreadcrumbsItem name="staffType_edit">
                    {this.state.id !== '0' ? 'Edit Staff Type' : 'New Staff Type'}
                </BreadcrumbsItem>
                
                <Spin spinning={this.state.loading}>
                    {this.renderForm()}
                </Spin>
            </Space>
        );
    }

    private fetchData = async () => {
        if (this.state.id === '0') {
            return;
        }

        this.setState({ loading: true });
        try {
            const result = await StaffTypeApiController.get(this.state.id);

            this.formRef?.setFieldsValue({
                ...result.data
            });
            this.setState({
                loading: false,
                staffType: result.data
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
        const dto = StaffTypeDTO.create({
            staffTypeId: this.state.id !== '0' ? this.state.id : undefined,
            name: values.name,
            isActive: values.isActive,
            displayOrder: values.displayOrder
        });

        this.saveStaffType(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { staffType } = this.state;

        return {
            name: staffType.name,
            isActive: staffType.isActive,
            displayOrder: staffType.displayOrder,
            allowMultiple: staffType.allowMultiple,
            isSupervisor: staffType.isSupervisor
        };
    }

    private saveStaffType = async (dto: StaffTypeDTO) => {
        this.setState({ loading: true });
        try {
            const result = await StaffTypeApiController.post(dto);

            this.setState({
                loading: false,
                // staffType: result.data,
            });
            HistoryUtil.push(Routes.GET.STAFF_TYPE_BASE);
        } catch (err) {
            this.setState({ loading: false });
            console.error(err);
            notification.error({
                message: err.message,
                description: err.description
            });
        }
    }
}

export default StaffTypeEdit;