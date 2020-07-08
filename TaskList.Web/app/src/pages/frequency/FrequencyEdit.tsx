import React from 'react'
import { RouteComponentProps } from 'react-router-dom';
import {
    notification,
    Spin,
    Form,
    Space,
} from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import { FormInstance } from 'antd/lib/form';
import * as FormHelper from '../../utils/FormUtil';
import FrequencyApiController from 'api/FrequencyApiController';
import FrequencyDTO from 'models/generated/FrequencyDTO';
import Routes from 'config/ConfigureRoutes';

interface RouteParams {
    id: string;
}

interface FrequencyEditState {
    id: string;
    loading: boolean;
    frequency: FrequencyDTO;
}

class FrequencyEdit extends React.Component<RouteComponentProps<RouteParams>, FrequencyEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: RouteComponentProps<RouteParams>) {
        super(props);
        const id = props.match == null ? '0' : props.match.params.id || '0';

        this.state = {
            loading: false,
            id,
            frequency: FrequencyDTO.create()
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
                {FormHelper.renderDisplayOrderFormItem()}
                {FormHelper.renderIsActiveFormItem()}
                {FormHelper.renderFormSaveButton()}
            </Form>
        );
    }

    render() {
        return (
            <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
                {/* <BreadcrumbsItem name="frequency_edit">
                    {this.state.id !== '0' ? 'Edit Frequency' : 'New Frequency'}
                </BreadcrumbsItem> */}
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
            const result = await FrequencyApiController.get(this.state.id);

            this.formRef?.setFieldsValue({
                ...result.data
            });
            
            this.setState({ 
                loading: false,
                frequency: result.data,
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

        const dto = FrequencyDTO.create({
            frequencyId: this.state.id !== '0' ? this.state.id : undefined,
            name: values.name,
            isActive: values.isActive,
            displayOrder: values.displayOrder
        });
       
        this.saveFrequency(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { frequency } = this.state;

        return {
            name: frequency.name,
            isActive: frequency.isActive,
            displayOrder: frequency.displayOrder,
        };
    }

    private saveFrequency = async (dto: FrequencyDTO) => {
        this.setState({ loading: true });
        try {
            const result = await FrequencyApiController.post(dto);

            this.setState({ loading: false });
            HistoryUtil.push(Routes.GET.FREQUENCY_BASE);
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

export default FrequencyEdit;