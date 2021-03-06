import React from 'react'
import { RouteComponentProps } from 'react-router-dom';
import {
    notification,
    Spin,
    Form,
    Space,
    Divider,
    Typography,
} from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import { FormInstance } from 'antd/lib/form';
import * as FormHelper from '../../utils/FormUtil';
import SubAreaTable from '../subArea/SubAreaTable';
import AreaDTO from 'models/generated/AreaDTO';
import AreaApiController from 'api/AreaApiController';
import Routes from 'config/ConfigureRoutes';
import { BreadcrumbsItem } from 'pages/shared/GlobalBreadcrumb';

interface RouteParams {
    id: string;
}

interface AreaEditState {
    id: string;
    loading: boolean;
    area: AreaDTO;
}

class AreaEdit extends React.Component<RouteComponentProps<RouteParams>, AreaEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: RouteComponentProps<RouteParams>) {
        super(props);

        const id = props.match == null ? '0' : props.match.params.id || '0';

        this.state = {
            loading: false,
            id,
            area: AreaDTO.create()
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

    renderSubAreaStuff = () => {
        return (
            <div>
                <Divider />
                <Typography.Title level={4}>Sub Areas</Typography.Title>
                <SubAreaTable
                    areaId={this.state.id}
                    areaName={this.state.area.name}
                    subAreas={this.state.area?.subAreas}
                />
            </div>
        );
    }

    render() {
        return (
            <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
                <BreadcrumbsItem name="area_edit" to={Routes.AREA_EDIT(this.state.area.areaId)}>
                    {this.state.id !== '0' ? 'Edit Area' : 'New Area'}
                </BreadcrumbsItem>

                <Spin spinning={this.state.loading}>
                    {this.renderForm()}
                    {this.state.id !== '0' && this.renderSubAreaStuff()}
                </Spin>
            </Space>
        );
    }

    private fetchData = async () => {
        if (this.state.id === '0') return;

        this.setState({ loading: true });

        try {
            const result = await AreaApiController.get(this.state.id);

            this.formRef?.setFieldsValue({
                ...result.data
            });

            this.setState({
                loading: false,
                area: result.data
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

        const dto = AreaDTO.create({
            areaId: this.state.id !== '0' ? this.state.id : undefined,
            ...values,
        });

        this.saveArea(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { area } = this.state;

        return {
            name: area.name,
            isActive: area.isActive,
            displayOrder: area.displayOrder,
        };
    }

    private saveArea = async (dto: AreaDTO) => {
        this.setState({ loading: true });
        try {
            const result = await AreaApiController.post(dto);

            this.setState({ loading: false });

            // If creating, go straight to edit with the returned id
            if(this.state.id === '0') {
                HistoryUtil.push(Routes.AREA_EDIT(result.data).ToRoute());
            } else {
                HistoryUtil.push(Routes.GET.AREA_BASE);
            }
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

export default AreaEdit;