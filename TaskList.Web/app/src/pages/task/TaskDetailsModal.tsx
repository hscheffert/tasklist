import React from 'react';
import { Row, Col, notification, Spin, Typography, Divider, Button } from 'antd';
import Modal from 'antd/lib/modal/Modal';
import { formatDateTime } from '../../utils/DateTimeHelper';
import TaskApiController from 'api/TaskApiController';
import TaskDetailsDTO from 'models/generated/TaskDetailsDTO';
import TaskStaffDTO from 'models/generated/TaskStaffDTO';
import Routes from 'config/ConfigureRoutes';
import HistoryUtil from '../../utils/HistoryUtil';

interface TaskDetailsModalProps {
    id: string;
    visible: boolean;
    closeModal: () => void;
    staffTypes: any[];
    isAdmin: boolean;
}

interface TaskDetailsModalState {
    loading: boolean;
    task: TaskDetailsDTO;
}

class TaskDetailsModal extends React.Component<TaskDetailsModalProps, TaskDetailsModalState> {
    constructor(props: TaskDetailsModalProps) {
        super(props);
        this.state = {
            loading: false,
            task: TaskDetailsDTO.create()
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

    renderRow = (label: string, value: any, key?: string) => {
        const labelProps = { span: 8 };
        const valueProps = { span: 16 };

        return (
            <Row key={key || label} style={{ marginTop: 6 }}>
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
        if (!this.props.staffTypes.length) return null;

        const taskStaffRows = this.props.staffTypes.flatMap(staffType => {
            const taskStaffOfThisStaffType = this.state.task?.taskStaff?.filter((ts: TaskStaffDTO) => ts.staffTypeId == staffType.staffTypeId);

            // If the task doesn't have any staff of this type, make sure we at least show it as empty
            if (!taskStaffOfThisStaffType.length) {
                return [{
                    key: staffType.staffTypeId,
                    staffTypeName: staffType.name,
                    name: 'None'
                }];
            }

            return [...taskStaffOfThisStaffType.map((ts: TaskStaffDTO) => {
                return {
                    key: ts.staffId,
                    staffTypeName: staffType.name,
                    name: ts.name
                };
            })];
        });

        return taskStaffRows.map((ts: any) => this.renderRow(ts.staffTypeName, ts.name, ts.key));
    }

    render() {
        if (!this.props.visible) {
            return null;
        }

        const { closeModal, visible, isAdmin } = this.props;
        const { task } = this.state;
        const editButton = (
            <Button onClick={() => HistoryUtil.push(Routes.TASK_EDIT(this.state.task.taskId as string).ToRoute())}>
                Edit
            </Button>
        );

        return (
            <Modal
                title={'Task Details'}
                visible={visible}
                onOk={closeModal}
                cancelButtonProps={{ style: { display: 'none' } }} // Hide cancel button
                onCancel={closeModal}
                footer={[
                    isAdmin && editButton,
                    <Button type="primary" onClick={closeModal}>
                        OK
                    </Button>,
                ]}>
                <Spin spinning={this.state.loading}>
                    {this.renderRow('Name', task.name)}
                    {this.renderRow('Area', task.areaName)}
                    {this.renderRow('Sub Area', task.subAreaName)}
                    {this.renderRow('Frequency', task.frequencyName)}
                    {this.renderRow('Notes', task.notes)}
                    {this.renderRow('Policy Tech', task.isInPolicyTech ? 'Yes' : 'No')}
                    {this.renderRow('Procedure File Name', task.procedureFileName || 'None')}
                    {this.renderRow('Is Active', task.isActive ? 'Yes' : 'No')}

                    <Divider />
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
