import React from 'react';
import { Row, Col, notification, Spin, Typography, Divider } from 'antd';
import Modal from 'antd/lib/modal/Modal';
import { formatDateTime } from '../../utils/DateTimeHelper';
import TaskApiController from 'api/TaskApiController';

interface TaskDetailsModalProps {
    id: string;
    visible: boolean;
    closeModal: () => void;
    staffTypes: any[];
}

interface TaskDetailsModalState {
    loading: boolean;
    task: any;
}

class TaskDetailsModal extends React.Component<TaskDetailsModalProps, TaskDetailsModalState> {
    constructor(props: TaskDetailsModalProps) {
        super(props);
        this.state = {
            loading: false,
            task: {}
        };
    }

    componentDidMount() {
        this.fetchData();
    }

    componentDidUpdate(prevProps: TaskDetailsModalProps) {
        if (prevProps.id !== this.props.id) {
            this.fetchData();
        }
    }

    renderRow = (label: string, value: any) => {
        const labelProps = { span: 8 };
        const valueProps = { span: 16 };

        return (
            <Row key={label} style={{ marginTop: 6 }}>
                <Col {...labelProps}>
                    <b>{label}</b>
                </Col>
                <Col {...valueProps}>
                    {value}
                </Col>
            </Row>
        )
    }

    renderTaskStaff = () => {
        const taskStaffRows = this.props.staffTypes?.map(staffType => ({
            staffTypeName: staffType.name,
            name: this.state.task?.taskStaff?.find((x: any) => x.staffTypeId == staffType.staffTypeId)?.name || '',
        }));

        return taskStaffRows.map((ts: any) => this.renderRow(ts.staffTypeName, ts.name));
    }

    render() {
        if (!this.props.visible) {
            return null;
        }

        const { closeModal, visible } = this.props;
        const { task } = this.state;

        return (
            <Modal
                title={'Task Details'}
                visible={visible}
                onOk={closeModal}
                cancelButtonProps={{ style: { display: 'none' } }} // Hide cancel button
                onCancel={closeModal}>
                <Spin spinning={this.state.loading}>
                    {this.renderRow('Name', task.name)}
                    {this.renderRow('Area', task.areaName)}
                    {this.renderRow('Sub Area', task.subAreaName)}
                    {this.renderRow('Frequency', task.frequencyName)}
                    {this.renderRow('Notes', task.notes)}
                    {this.renderRow('Policy Tech', task.isInPolicyTech ? 'Yes' : 'No')}
                    {this.renderRow('Procedure File Name', task.procedureFileName || 'None')}
                    {this.renderRow('Is Active', task.isActive ? 'Yes' : 'No')}

                    <Divider/>
                    <Typography.Title level={4}>Staff</Typography.Title>
                    {this.renderTaskStaff()}

                    {task.updatedDate && task.updatedBy && <div style={{ marginTop: 20 }}>
                         <i>{`Last Updated on ${formatDateTime(task.updatedDate)} by ${task.updatedBy}`}</i>
                    </div>}
                </Spin>

            </Modal>
        );
    }

    private fetchData = async () => {
        if (!this.props.id) {
            return;
        }

        this.setState({ loading: true });
        try {
            const result = await TaskApiController.getWithDetails(this.props.id);

            this.setState({
                loading: false,
                task: result.data
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
}

export default TaskDetailsModal;
