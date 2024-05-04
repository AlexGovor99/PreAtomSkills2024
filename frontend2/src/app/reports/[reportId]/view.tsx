"use client";

import { Button, Col, Flex, Modal, Row, Space, Typography } from "antd";
import { NextPage } from "next";
import { useRouter } from "next/navigation";
import { useUser } from "../../../../user";
import Link from "next/link";
import { useState } from "react";
import TextArea from "antd/es/input/TextArea";

const getKeyLabel = (key: string) => {
  switch (key) {
    case "id": {
      return "Report ID";
    }

    case "name": {
      return "Report name";
    }

    case "userId": {
      return "Scientist (ID)";
    }

    case "status": {
      return "Status";
    }

    case "editDate": {
      return "Modified at";
    }

    case "comment": {
      return "Rejection reason";
    }

    case "fileId": {
      return "Attachment";
    }

    default: {
      return `Unknown field (${key})`;
    }
  }
};

enum ReportStatus {
  Created, // 0
  Approved, // 1
  Rejected, // 2
  Sent, // 3
}

const getValue = (key: string, value: any) => {
  switch (key) {
    case "id": {
      return value;
    }

    case "name": {
      return value;
    }

    case "userId": {
      return value;
    }

    case "status": {
      return ReportStatus[value];
    }

    case "editDate": {
      return new Date(Date.parse(value)).toLocaleString();
    }

    case "comment": {
      return value || "-";
    }

    case "fileId": {
      return "coming-soon.pdf";
    }

    default: {
      return `Unknown field (${key})`;
    }
  }
};

const ReportView = ({ data, reportId }: any) => {
  const router = useRouter();
  const user = useUser();
  const list = Object.entries(data);
  const [rejectionComment, setRejectionComment] = useState("");

  const [isModalOpen, setIsModalOpen] = useState(false);

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleOk = async () => {
    const response = await fetch("http://localhost:5002/order/reject", {
      method: "POST", // *GET, POST, PUT, DELETE, etc.
      mode: "cors", // no-cors, *cors, same-origin
      cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
      credentials: "same-origin", // include, *same-origin, omit
      headers: new Headers([["Content-Type", "application/json"]]),
      redirect: "follow", // manual, *follow, error
      referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      body: JSON.stringify({
        userId: user?.id,
        orderId: data.id,
        comment: rejectionComment,
        // request: { ...values, userId: user.id, fileId: "no-file-yet", request: {} },
      }), // body data type must match "Content-Type" header
    });

    if (!response.ok) {
      return;
    }

    // const result = await response.json();

    setIsModalOpen(false);
    router.push("/reports");
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  return (
    <>
      <Row>
        <Col offset={6} span={12}>
          <Typography.Title>Report summary</Typography.Title>
        </Col>
      </Row>
      <ul style={{ marginTop: "20px" }}>
        {list.map(([key, value]: any) => {
          return (
            <li key={key} style={{ marginBottom: "2em" }}>
              <Row>
                <Col offset={6} span={3}>
                  <Typography.Text strong>{getKeyLabel(key)}</Typography.Text>
                </Col>
                <Col span={9}>
                  <Typography.Text>{getValue(key, value)}</Typography.Text>
                </Col>
              </Row>
            </li>
          );
        })}
      </ul>
      <Row style={{ marginTop: "40px" }} justify="start">
        <Col offset={6} span={12}>
          <Flex justify="space-between">
            <Space>
              {user?.role === "scientist" ? (
                <Link href={`/reports/${reportId}/edit`}>
                  <Button
                    type="primary"
                    disabled={![ReportStatus.Created, ReportStatus.Rejected].includes(data.status)}
                  >
                    Edit
                  </Button>
                </Link>
              ) : null}
              {user?.role === "inspector" ? (
                <Button
                  type="primary"
                  disabled={data.status !== ReportStatus.Created}
                  onClick={async () => {
                    const response = await fetch("http://localhost:5002/order/approve", {
                      method: "POST", // *GET, POST, PUT, DELETE, etc.
                      mode: "cors", // no-cors, *cors, same-origin
                      cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
                      credentials: "same-origin", // include, *same-origin, omit
                      headers: new Headers([["Content-Type", "application/json"]]),
                      redirect: "follow", // manual, *follow, error
                      referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
                      body: JSON.stringify({
                        userId: user?.id,
                        orderId: data.id,
                        // request: { ...values, userId: user.id, fileId: "no-file-yet", request: {} },
                      }), // body data type must match "Content-Type" header
                    });

                    if (!response.ok) {
                      return;
                    }

                    // const result = await response.json();

                    router.push("/reports");
                  }}
                >
                  Approve
                </Button>
              ) : null}
              {user?.role === "inspector" ? (
                <Button danger onClick={showModal}>
                  Reject
                </Button>
              ) : null}
            </Space>
            <Button type="link" onClick={() => router.push("/reports")}>
              Back to reports
            </Button>
          </Flex>
        </Col>
      </Row>{" "}
      <Modal
        title="Enter rejection comment"
        open={isModalOpen}
        onOk={handleOk}
        onCancel={handleCancel}
        okButtonProps={{
          disabled: !rejectionComment.length,
        }}
      >
        <TextArea onChange={(e) => setRejectionComment(e.target.value)} value={rejectionComment} />
      </Modal>
    </>
  );
};

export default ReportView;
