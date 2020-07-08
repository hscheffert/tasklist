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
import SubAreaDTO from 'models/generated/SubAreaDTO';
import SubAreaApiController from 'api/SubAreaApiController';
import Routes from 'config/ConfigureRoutes';

interface RouteParams {
    id: string;
    areaId: string;
}

interface SubAreaEditState {
    id: string;
    areaId: string;
    loading: boolean;
    subArea: SubAreaDTO;
}

class SubAreaEdit extends React.Component<RouteComponentProps<RouteParams>, SubAreaEditState> {
    private formRef: FormInstance | null | undefined;
    constructor(props: RouteComponentProps<RouteParams>) {
        super(props);
        
        const id = props.match == null ? '0' : props.match.params.id || '0';
        const areaId = props.match == null ? '0' : props.match.params.areaId || '0';

        this.state = {
            loading: false,
            id,
            areaId,
            subArea: SubAreaDTO.create()
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
                {/* <BreadcrumbsItem name="subArea_edit">
                    {this.state.id !== '0' ? 'Edit Sub Area' : 'New Sub Area'}
                </BreadcrumbsItem> */}
                <Spin spinning={this.state.loading}>
                    {this.renderForm()}
                </Spin>
            </Space>
        );
    }

    private fetchData = async () => {
        if(this.state.id === '0') return;
        
        this.setState({ loading: true });
        try {
            const result = await SubAreaApiController.get(this.state.id);
            
            this.formRef?.setFieldsValue({
                ...result.data
            });
            
            this.setState({ 
                loading: false,
                subArea: result.data,
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

        const dto = SubAreaDTO.create({
            subAreaId: this.state.id !== '0' ? this.state.id : undefined,
            areaId: this.state.areaId,
            name: values.name,
            isActive: values.isActive,
            displayOrder: values.displayOrder
        });
       
        this.saveSubArea(dto);
    }

    private getInitialValues = () => {
        if (this.state.id === '0') {
            return {
                isActive: true
            };
        }

        const { subArea } = this.state;

        return {
            name: subArea.name,
            isActive: subArea.isActive,
            displayOrder: subArea.displayOrder,
        };
    }

    private saveSubArea = async (dto: SubAreaDTO) => {
        this.setState({ loading: true });
        try {
            const result = await SubAreaApiController.post(dto);
                    
            this.setState({ 
                loading: false
             });

             // Go back to Area edit
            HistoryUtil.push(Routes.AREA_EDIT(this.state.subArea.areaId).ToRoute());            
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

export default SubAreaEdit;